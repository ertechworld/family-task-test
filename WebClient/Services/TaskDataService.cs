using Domain.Commands;
using Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebClient.Abstractions;
using Microsoft.AspNetCore.Components;
using Domain.ViewModel;
using Core.Extensions.ModelConversion;
using WebClient.Shared.Models;


namespace WebClient.Services
{
    public class TaskDataService : ITaskDataService
    {
        private readonly HttpClient httpClient;
        public TaskDataService(IHttpClientFactory clientFactory)
        {
            httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            tasks = new List<TaskVm>();
            members = new List<MemberVm>();
            LoadTasks();
        }
        private IEnumerable<TaskVm> tasks;

        public IEnumerable<TaskVm> Tasks => tasks;

        private IEnumerable<MemberVm> members;

        public IEnumerable<MemberVm> Members => members;
        public TaskVm SelectedTask { get; private set; }

        public event EventHandler TasksChanged;
        public event EventHandler TasksUpdated;
        public event EventHandler<string> UpdateTaskFailed;
        public event EventHandler<string> CreateTaskFailed;
        public event EventHandler SelectedTaskChanged;
        public event EventHandler TaskSelected;

        public async void LoadTasks()
        {
            tasks = (await GetAllTasks()).Payload;
            members = (await GetAllMembers()).Payload;
            foreach (var item in tasks)
            {
                var member = members.FirstOrDefault(memberVm => memberVm.Id == item.AssignedToId);
                if (member != null)
                {
                    item.Member = new Domain.DataModels.Member()
                    {
                        Avatar = member.Avatar
                    };
                }


            }
            TasksChanged?.Invoke(this, null);
        }


        private async Task<CreateTaskCommandResult> Create(CreateTaskCommand command)
        {
            return await httpClient.PostJsonAsync<CreateTaskCommandResult>("tasks", command);
        }

        private async Task<GetAllTasksQueryResult> GetAllTasks()
        {
            return await httpClient.GetJsonAsync<GetAllTasksQueryResult>("tasks");
        }
        private async Task<GetAllMembersQueryResult> GetAllMembers()
        {
            return await httpClient.GetJsonAsync<GetAllMembersQueryResult>("members");
        }


        private async Task<UpdateTaskCommandResult> Update(UpdateTaskCommand command)
        {
            return await httpClient.PutJsonAsync<UpdateTaskCommandResult>($"tasks/{command.Id}", command);
        }

        public void SelectTask(Guid id)
        {
            if (tasks.All(taskvm => taskvm.Id != id)) return;
            {
                SelectedTask = tasks.SingleOrDefault(taskvm => taskvm.Id == id);
                SelectedTaskChanged?.Invoke(this, null);
            }
        }

        public void SelectNullMember()
        {
            SelectedTask = null;
            SelectedTaskChanged?.Invoke(this, null);
        }

        public async Task UpdateTask(TaskVm model)
        {
            var result = await Update(model.ToUpdateTaskCommand());

            Console.WriteLine(JsonSerializer.Serialize(result));

            if (result != null)
            {
                var updatedList = (await GetAllTasks()).Payload;

                if (updatedList != null)
                {
                    tasks = updatedList;
                    TasksChanged?.Invoke(this, null);
                    return;
                }
                UpdateTaskFailed?.Invoke(this, "The save was successful, but we can no longer get an updated list of members from the server.");
            }

            UpdateTaskFailed?.Invoke(this, "Unable to save changes.");
        }

        public async Task CreateTask(TaskVm model)
        {
            var result = await Create(model.ToCreateTaskCommand());
            if (result != null)
            {
                var updatedList = (await GetAllTasks()).Payload;

                if (updatedList != null)
                {
                    tasks = updatedList;
                    TasksChanged?.Invoke(this, null);
                    return;
                }
                UpdateTaskFailed?.Invoke(this, "The creation was successful, but we can no longer get an updated list of members from the server.");
            }

            UpdateTaskFailed?.Invoke(this, "Unable to create record.");
        }

        public void ToggleTask(Guid id)
        {
            foreach (var taskModel in Tasks)
            {
                if (taskModel.Id == id)
                {
                    taskModel.IsComplete = !taskModel.IsComplete;

                    var result = Update(taskModel.ToUpdateTaskCommand());
                }
            }

            TasksUpdated?.Invoke(this, null);
        }
        public void DropTask(Guid memberId, Guid taskId)
        {
            foreach (var taskModel in Tasks)
            {
                if (taskModel.Id == taskId)
                {
                    taskModel.AssignedToId = memberId;

                    var result = Update(taskModel.ToUpdateTaskCommand());
                }
            }

            TasksUpdated?.Invoke(this, null);
        }
        public async void SelectedMemberTask(Guid memberId)
        {
            tasks = (await GetAllTasks()).Payload;
            tasks = tasks.Where(memberVm => memberVm.AssignedToId == memberId).ToList();
            SelectedTaskChanged?.Invoke(this, null);

        }

    }
}