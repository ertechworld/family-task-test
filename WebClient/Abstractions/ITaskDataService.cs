using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Shared.Models;

namespace WebClient.Abstractions
{
    /// <summary>
    /// This Service is currently using the TaskModel Class, and will need to use a shared view
    /// model after the model has been created.  For the moment, this pattern facilitates a client
    /// side storage mechanism to view functionality.  See work completed for the MemberDataService
    /// for an example of expectations.
    /// </summary>
    public interface ITaskDataService
    {
        IEnumerable<TaskVm> Tasks { get; }
        TaskVm SelectedTask { get; }
        event EventHandler TasksChanged;
        event EventHandler SelectedTaskChanged;
        //event EventHandler<string> UpdateMemberFailed;
        //event EventHandler<string> CreateMemberFailed;

        void LoadTasks();
        event EventHandler<string> UpdateTaskFailed;
        event EventHandler<string> CreateTaskFailed;
        //List<TaskModel> Tasks { get; }
        //TaskModel SelectedTask { get; }
        Task UpdateTask(TaskVm model);
        Task CreateTask(TaskVm model);
        event EventHandler TasksUpdated;
        event EventHandler TaskSelected;
        //Task UpdateTask(TaskVm model);
       // Task AddTask(TaskVm model);
        void SelectTask(Guid id);
        void ToggleTask(Guid id);
        void DropTask(Guid memberId, Guid taskId);
        void SelectedMemberTask(Guid memberId);
        //void AddTask(TaskModel model);
    }
}