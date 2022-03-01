﻿using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Models;
using BookStoreApi.Services;
namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class LogController : ControllerBase
    {
        private readonly LogsService _logService;
        public LogController(LogsService logService)
        {
            this._logService = logService;
        }
        [HttpGet("all")]
        public async Task<List<Logs>> GetLogs() => await this._logService.GetLogs();
        [HttpGet("logLevel/{logLevel}")]
        public async Task<List<Logs>> GetLogsByLogLevel(int logLevel) => await this._logService.GetLogsByLogLevel(logLevel);
    }
}