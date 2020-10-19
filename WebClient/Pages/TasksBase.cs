using System;
using System.Collections.Generic;
using Domain.ViewModel;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebClient.Abstractions;
using WebClient.Services;
using WebClient.Shared.Models;

namespace WebClient.Pages
{
    public class TasksBase : ComponentBase
    {
        protected List<TaskVm> tasks = new List<TaskVm>();
        protected List<TaskModel> leftMenuItem = new List<TaskModel>();

        protected bool showCreator;
        protected bool isLoaded;

        [Inject]
        public ITaskDataService TaskDataService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            UpdateTasks();
            ReloadMenu();

            TaskDataService.TasksChanged += TaskDataService_TasksChanged;
            showCreator = true;
            isLoaded = true;
        }
        private void TaskDataService_TasksChanged(object sender, EventArgs e)
        {
            UpdateTasks();
            ReloadMenu();

            showCreator = true;
            isLoaded = true;

            StateHasChanged();
        }

        void UpdateTasks()
        {
            var result = TaskDataService.Tasks;

            if (result.Any())
            {
                tasks = result.ToList();
            }
        }

        void ReloadMenu()
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                leftMenuItem.Add(new TaskModel
                {
                    IsDone = tasks[i].IsComplete,
                    Text = tasks[i].Subject,
                    Id = tasks[i].Id
                });
            }
        }

        protected void onAddItem()
        {
            showCreator = true;
            StateHasChanged();
        }

        protected void onTaskAdd(TaskVm task)
        {
            TaskDataService.CreateTask(task);
        }
    }
}
