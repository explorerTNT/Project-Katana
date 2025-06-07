using UnityEngine;
using System.Collections.Generic;

public class DebugConsole : MonoBehaviour
{
    public bool IsConsoleActive { get; private set; } = false; // ��������� ������� (���������)
    [SerializeField] private EnemySpawner enemySpawner; // ������ ������ �� EnemySpawner
    private string inputString = "";                   // ������� ����
    private List<string> logMessages = new List<string>(); // ������� �����
    private Vector2 scrollPosition;                   // ������� ��������� �����
    private Dictionary<string, System.Action<string[]>> commands; // ������� ������
    private bool isFirstOpen = true;                  // ���� ������� �������� �������
    private bool showFps = false;                     // ���� ����������� FPS
    private float deltaTime = 0.0f;                   // ��� ������� FPS

    void Awake()
    {
        commands = new Dictionary<string, System.Action<string[]>>();
        RegisterCommands();
    }

    void Update()
    {
        // ��������� deltaTime ��� ������� FPS (�����������)
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    private void ToggleConsole(string source)
    {
        IsConsoleActive = !IsConsoleActive;
        if (IsConsoleActive)
        {
            Time.timeScale = 0f; // ����� ����
            inputString = ""; // ������� ���� �����
            GUI.FocusControl("ConsoleInput"); // ����� �� ���� �����
            // ���������� ������ Unity ��� ������ ��������
            if (isFirstOpen)
            {
                Log($"Unity Engine Version: {Application.unityVersion}");
                isFirstOpen = false;
            }
        }
        else
        {
            Time.timeScale = 1f; // ������������� ����
            GUI.FocusControl(null); // ������ ������
        }
    }

    void OnGUI()
    {
        // ������������ ������� � OnGUI
        if (Event.current.isKey && Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.BackQuote)
            {
                ToggleConsole("BackQuote (OnGUI)");
                Event.current.Use();
            }
            else if (Event.current.keyCode == KeyCode.F1)
            {
                ToggleConsole("F1 (OnGUI)");
                Event.current.Use();
            }
        }

        // ����������� FPS, ���� ��������
        if (showFps)
        {
            float fps = 1.0f / deltaTime;
            GUI.Label(new Rect(10, 10, 100, 20), $"FPS: {Mathf.Round(fps)}");
        }

        if (!IsConsoleActive) return;

        // ��� ������� (��������������)
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height / 2), "");

        // ���� ��� �����
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height / 2 - 50));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        foreach (string log in logMessages)
        {
            GUILayout.Label(log);
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        // ���� �����
        GUI.SetNextControlName("ConsoleInput");
        Rect inputRect = new Rect(10, Screen.height / 2 - 30, Screen.width - 20, 20);
        inputString = GUI.TextField(inputRect, inputString);

        // ��������� �� ���� ����� ��� �������� �������
        if (IsConsoleActive && GUI.GetNameOfFocusedControl() != "ConsoleInput")
        {
            GUI.FocusControl("ConsoleInput");
        }

        // ��������� Enter ��� �������� �������
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && !string.IsNullOrEmpty(inputString))
        {
            ProcessCommand(inputString);
            inputString = "";
            Event.current.Use();
        }
    }

    void RegisterCommands()
    {
        // ������� ������ �����
        commands.Add("spawn_enemy", (args) =>
        {
            int count = 1;
            if (args.Length > 0 && int.TryParse(args[0], out int parsedCount))
            {
                count = parsedCount;
            }
            if (enemySpawner == null)
            {
                Log("Error: EnemySpawner not assigned in DebugConsole.");
                return;
            }
            if (enemySpawner.enemyPrefab == null)
            {
                Log("Error: Enemy prefab not assigned in EnemySpawner.");
                return;
            }
            if (enemySpawner.spawnPoints == null || enemySpawner.spawnPoints.Length == 0)
            {
                Log("Error: No spawn points assigned in EnemySpawner.");
                return;
            }
            for (int i = 0; i < count; i++)
            {
                enemySpawner.SpawnEnemy();
            }
            Log($"Spawned {count} enemy(ies).");
        });

        // ������� ��������� �������� ������
        commands.Add("set_speed", (args) =>
        {
            if (args.Length > 0 && float.TryParse(args[0], out float newSpeed))
            {
                PlayerMovement player = FindObjectOfType<PlayerMovement>();
                if (player != null)
                {
                    player.walkSpeed = newSpeed;
                    Log($"Player walk speed set to {newSpeed}.");
                }
                else
                {
                    Log("Error: Player not found.");
                }
            }
            else
            {
                Log("Usage: set_speed <value>");
            }
        });

        // ������� ��� ���������/���������� ����������� FPS
        commands.Add("show_fps", (args) =>
        {
            if (args.Length > 0 && int.TryParse(args[0], out int value))
            {
                if (value == 1)
                {
                    showFps = true;
                    Log("FPS display enabled.");
                }
                else if (value == 0)
                {
                    showFps = false;
                    Log("FPS display disabled.");
                }
                else
                {
                    Log("Usage: show_fps <0 or 1>");
                }
            }
            else
            {
                Log("Usage: show_fps <0 or 1>");
            }
        });

        // ������� ��� ������ ������ ���� ������
        commands.Add("help", (args) =>
        {
            Log("Available commands:");
            Log("  spawn_enemy [count] - Spawns [count] enemies (default 1).");
            Log("  set_speed <value> - Sets player walk speed to <value>.");
            Log("  show_fps <0 or 1> - Toggles FPS display (1 = on, 0 = off).");
            Log("  help - Shows this list of commands.");
        });
    }

    void ProcessCommand(string input)
    {
        string[] parts = input.Trim().Split(' ');
        if (parts.Length == 0) return;

        string command = parts[0].ToLower();
        string[] args = new string[parts.Length - 1];
        for (int i = 1; i < parts.Length; i++)
        {
            args[i - 1] = parts[i];
        }

        if (commands.ContainsKey(command))
        {
            commands[command].Invoke(args);
        }
        else
        {
            Log($"Unknown command: {command}");
        }
    }

    void Log(string message)
    {
        logMessages.Add(message);
        scrollPosition.y = Mathf.Infinity; // ��������� ����
    }
}