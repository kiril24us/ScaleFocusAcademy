using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.DTO.Requests.WorkLogRequests;
using ProjectManagementApplication.DTO.Responses.WorkLogResponses;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Api.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class WorkLogsController : Controller
    {
        private readonly IWorkLogService _workLogService;
        private readonly IUserManager _userManager;

        public WorkLogsController(IWorkLogService workLogService, IUserManager userManager)
        {
            _workLogService = workLogService;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Policy = "AssigneeOnly")]
        public async Task<IActionResult> Create(WorkLogCreateRequestDTO workLog)
        {
            User currentUser = await _userManager.GetTheUserAsync(HttpContext.User);

            Tuple<Messages,int> tuple =  await _workLogService.CreateWorkLog(workLog.TaskId, workLog.Description, workLog.TimeSpent, workLog.StartDate, currentUser.Id);

            switch (tuple.Item1)
            {            
                case Messages.TaskNotFound:
                    return BadRequest($"Be sure that Task with Id {workLog.TaskId} is assigned to you");

                case Messages.Success:
                    WorkLog workLogFromDb = await _workLogService.GetUserWorkLogById(tuple.Item2, workLog.TaskId, currentUser.Id);
                    WorkLogCreateResponseDTO workLogCreateResponseDTO = new WorkLogCreateResponseDTO();
                    MapWorkLog(workLogFromDb, workLogCreateResponseDTO);
                    return CreatedAtAction("Create", workLogCreateResponseDTO);

                case Messages.OperationWasNotSuccessful:
                    return BadRequest("Operation was not successful");

                case Messages.AlreadyExistWorkLog:
                    return BadRequest("Already exist such Work Log");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpDelete]
        [Authorize(Policy = "AssigneeOnly")]
        [Route("{workLogId}")]
        public async Task<IActionResult> Delete(int workLogId, WorkLogDeleteRequestDTO workLogDeleteRequestDTO)
        {
            User currentUser = await _userManager.GetTheUserAsync(HttpContext.User);

            bool isSuccess = await _workLogService.DeleteWorkLog(workLogId, workLogDeleteRequestDTO.TaskId, currentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Be sure that Task with Id {workLogDeleteRequestDTO.TaskId} is assigned to you and workLog with Id {workLogId} exist");
        }

        [HttpPut]
        [Authorize(Policy = "AssigneeOnly")]
        [Route("{workLogId}")]
        public async Task<IActionResult> Edit(int workLogId, WorkLogEditRequestDTO workLogEditRequestDTO)
        {
            User currentUser = await _userManager.GetTheUserAsync(HttpContext.User);

            bool isSuccess = await _workLogService.EditWorkLog(workLogId, workLogEditRequestDTO.Description, workLogEditRequestDTO.TaskId, workLogEditRequestDTO.StartDate, workLogEditRequestDTO.TimeSpent, currentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Be sure that Task with Id {workLogEditRequestDTO.TaskId} is assigned to you and workLog with Id {workLogId} exist");
        }

        [HttpGet]
        [Authorize(Policy = "AssigneeOnly")]
        [Route("{taskId}")]
        public async Task<IActionResult> GetAll([FromRoute] int taskId)
        {
            User currentUser = await _userManager.GetTheUserAsync(HttpContext.User);

            List<WorkLogCreateResponseDTO> workLogsDTO = new List<WorkLogCreateResponseDTO>();

            foreach (WorkLog workLog in await _workLogService.GetAllWorkLogs(taskId, currentUser.Id))
            {
                WorkLogCreateResponseDTO dto = new WorkLogCreateResponseDTO();
                MapWorkLog(workLog, dto);
                workLogsDTO.Add(dto);
            }

            return Ok(workLogsDTO);
        }

        private void MapWorkLog(WorkLog workLogFromDb, WorkLogCreateResponseDTO workLogCreateResponseDTO)
        {
            workLogCreateResponseDTO.Id = workLogFromDb.Id;
            workLogCreateResponseDTO.Description = workLogFromDb.Description;
            workLogCreateResponseDTO.TimeSpent = workLogFromDb.TimeSpent;
            workLogCreateResponseDTO.StartDate = workLogFromDb.StartDate.ToString("d");
            workLogCreateResponseDTO.TaskId = workLogFromDb.TaskId;
            workLogCreateResponseDTO.UserId = workLogFromDb.UserId;
        }
    }
}
