using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TaskType
{
    UnscrewBolt,
    DisassembleBuilding
}

public class TaskManager : MonoBehaviour
{
    [System.Serializable]
    public class Task
    {
        public string description;
        public TaskType type;
        public ColorsEnum requiredColor;
        public int currentCount;
        public int targetCount;
        public bool completed;
        public Task nextTask;

        public Task(string desc, TaskType type, int target, ColorsEnum color)
        {
            description = desc;
            this.type = type;
            targetCount = target;
            currentCount = 0;
            completed = false;
            requiredColor = color;
        }

        public void Progress()
        {
            if (!completed)
            {
                currentCount++;
                if (currentCount >= targetCount)
                {
                    completed = true;
                }
            }
        }

        public override string ToString()
        {
            return $"{description}: {currentCount}/{targetCount}" + (completed ? " " : "");
        }
    }

    public TextMeshProUGUI tasksTextUI;
    public List<Task> tasks = new List<Task>();

    private void Start()
    {
        var red = Color.red;
        var yellow = Color.yellow;

        var redTask = new Task("Unscrew the red bolts", TaskType.UnscrewBolt, 5, ColorsEnum.red);
        var yellowTask = new Task("Unscrew the yellow bolts", TaskType.UnscrewBolt, 5, ColorsEnum.yellow);
        var dismantle = new Task("Dismantle the buildings", TaskType.DisassembleBuilding, 2, ColorsEnum.red);

        redTask.nextTask = yellowTask;
        yellowTask.nextTask = dismantle;

        tasks.Add(redTask);
        UpdateTaskUI();
    }

    public void ProgressBoltTask(ColorsEnum actualColor)
    {
        foreach (var task in tasks) {
            if (task.completed) continue;

            if (task.type == TaskType.UnscrewBolt) {
                if (task.requiredColor == actualColor) {
                    task.Progress();
                    Debug.Log($"Задание обновлено: {task.description} ({task.currentCount}/{task.targetCount})");

                    if (task.completed) {
                        Debug.Log($"Задание выполнено: {task.description}");
                        if (task.nextTask != null) {
                            tasks.Add(task.nextTask);
                            Debug.Log($"Новое задание: {task.nextTask.description}");
                        }
                    }

                    UpdateTaskUI();
                    break;
                }
            } else if (task.type == TaskType.DisassembleBuilding) {
                task.Progress();
                Debug.Log($"Задание обновлено: {task.description} ({task.currentCount}/{task.targetCount})");

                if (task.completed) {
                    Debug.Log($"Задание выполнено: {task.description}");
                    if (task.nextTask != null) {
                        tasks.Add(task.nextTask);
                        Debug.Log($"Новое задание: {task.nextTask.description}");
                    }
                }

                UpdateTaskUI();
                break;
            }
        }
    }

    public void ProgressGenericTask(TaskType type)
    {
        foreach (var task in tasks)
        {
            if (!task.completed && task.type == type)
            {
                task.Progress();
                Debug.Log($"Прогресс по заданию: {task.description} ({task.currentCount}/{task.targetCount})");

                if (task.completed) 
                {
                    Debug.Log($"Задание выполнено: {task.description}");
                    if (task.nextTask != null)
                    {
                        tasks.Add(task.nextTask);
                        Debug.Log($"Новое задание: {task.nextTask.description}");
                    }
                }

                UpdateTaskUI();
                break;
            }
        }
    }

    private void UpdateTaskUI()
    {
        if (tasksTextUI == null) return;

        tasksTextUI.text = "";
        foreach (var task in tasks)
        {
            tasksTextUI.text += task.ToString() + "\n";
        }
    }
}
