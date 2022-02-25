    ﻿using IntroSE.Kanban.Backend.BusinessLayer;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
    
    namespace IntroSE.Kanban.Backend.ServiceLayer
    {
        internal class BoardService
        {
            private BoardController boardController;
    
            public BoardController BoardController { get => boardController;}
    
            public BoardService()
            {
                boardController = new BoardController();
            }
    
          /// <summary>
          /// Limit the number of tasks in a specific column
          /// </summary>
          /// <param name="email">The email address of the user, must be logged in</param>
         /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
          /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
          /// <returns>A response object. The response should contain a error message in case of an error</returns>
            public Response LimitColumnTasks(string email, int columnOrdinal, int limit)
            {
                Response toReturn;
                try
                {
                    boardController.LimitColumnTasks(email, columnOrdinal, limit);
                    toReturn = new Response();
                }
                catch(Exception ee)
                {
                    toReturn = new Response(ee.Message);
                    
                }
                return toReturn;
            }
          /// <summary>
          /// Returns a column given it's name
          /// </summary>
          /// <param name="email">Email of the user. Must be logged in</param>
          /// <param name="columnName">Column name</param>
          /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
            public Response<Column> GetColumn(string email, string columnName)
            {
                Response<Column> toReturn;
                try
                {
                    BusinessLayer.BoardPackage.Column columnBusiness = boardController.GetColumn(email, columnName);
                    Column columnService = new Column(columnBusiness);
                    toReturn = new Response<Column>(columnService);
                }
                catch (Exception ee)
                {
                    toReturn = new Response<Column>(ee.Message);
                }
                return toReturn;
            }
          /// <summary>
          /// Returns a column given it's identifier.
          /// The first column is identified by 0, the ID increases by 1 for each column
          /// </summary>
          /// <param name="email">Email of the user. Must be logged in</param>
          /// <param name="columnOrdinal">Column ID</param>
          /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
            public Response<Column> GetColumn(string email, int columnOrdinal)
            {
                Response<Column> toReturn;
                try
                {
                    BusinessLayer.BoardPackage.Column columnBusiness = boardController.GetColumn(email, columnOrdinal);
                    Column columnService = new Column(columnBusiness);
                    toReturn = new Response<Column>(columnService);
                }
                catch (Exception ee)
                {
                    toReturn = new Response<Column>(ee.Message);
                }
                return toReturn;
            }
          /// <summary>
          /// Returns the board of a user. The user must be logged in
          /// </summary>
          /// <param name="email">The email of the user</param>
          /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error
            public Response<Board> GetBoard(string email)
            {
                Response<Board> toReturn;
                try
                {
                    Board boardService = new Board(boardController.GetBoard(email),BoardController.Boards[email].Email);
                    toReturn = new Response<Board>(boardService);
                }
                catch (Exception ee)
                {
                    toReturn = new Response<Board>(ee.Message);
                }
                return toReturn;
            }
            public Response<Task> AddTask(string email, string title, string description, DateTime dueDate)
            {
                Response<Task> toReturn;
                try
                {
                    BusinessLayer.BoardPackage.Task taskBusiness = boardController.AddTask(email, title, description, dueDate);
                    Task taskService = new Task(taskBusiness);
                    toReturn = new Response<Task>(taskService);
                }
                catch (Exception ee)
                {
                    toReturn = new Response<Task>(ee.Message);
                }
                return toReturn;
            }
          /// <summary>
          /// Update the due date of a task
          /// </summary>
          /// <param name="email">Email of the user. Must be logged in</param>
          /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
          /// <param name="taskId">The task to be updated identified task ID</param>
          /// <param name="dueDate">The new due date of the column</param>
          /// <returns>A response object. The response should contain a error message in case of an error</returns>
            public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
            {
                Response toReturn;
                try
                {
                    boardController.UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate);
                    toReturn = new Response();
                }
                catch (Exception ee)
                {
                    toReturn = new Response(ee.Message);
                }
                return toReturn;
            }
          /// <summary>
          /// Update task title
          /// </summary>
          /// <param name="email">Email of user. Must be logged in</param>
          /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
          /// <param name="taskId">The task to be updated identified task ID</param>
          /// <param name="title">New title for the task</param>
          /// <returns>A response object. The response should contain a error message in case of an error</returns>
            public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
            {
                Response toReturn;
                try
                {
                    boardController.UpdateTaskTitle(email, columnOrdinal, taskId, title);
                    toReturn = new Response();
                }
                catch (Exception ee)
                {
                    toReturn = new Response(ee.Message);
                }
                return toReturn;
            }
          /// <summary>
          /// Update the description of a task
          /// </summary>
          /// <param name="email">Email of user. Must be logged in</param>
          /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
          /// <param name="taskId">The task to be updated identified task ID</param>
          /// <param name="description">New description for the task</param>
          /// <returns>A response object. The response should contain a error message in case of an error</returns>
            public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
            {
                Response toReturn;
                try
                {
                    boardController.UpdateTaskDescription(email, columnOrdinal, taskId, description);
                    toReturn = new Response();
                }
                catch (Exception ee)
                {
                    toReturn = new Response(ee.Message);
                }
                return toReturn;
            }
          /// <summary>
          /// Advance a task to the next column
          /// </summary>
          /// <param name="email">Email of user. Must be logged in</param>
          /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
          /// <param name="taskId">The task to be updated identified task ID</param>
          /// <returns>A response object. The response should contain a error message in case of an error</returns>
            public Response AdvanceTask(string email, int columnOrdinal, int taskId)
            {
                Response toReturn;
                try
                {
                    boardController.AdvanceTask(email, columnOrdinal, taskId);
                    toReturn = new Response();
                }
                catch (Exception ee)
                {
                    toReturn = new Response(ee.Message);
                }
                return toReturn;
            }
          /// <summary>
          /// Removes a column given it's identifier.
          /// The first column is identified by 0, the ID increases by 1 for each column
          /// </summary>
          /// <param name="email">Email of the user. Must be logged in</param>
          /// <param name="columnOrdinal">Column ID</param>
          /// <returns>A response object. The response should contain a error message in case of an error</returns>
            public Response RemoveColumn(string email, int columnOrdinal)
            {
                Response toReturn;
                try
                {
                    boardController.RemoveColumn(email,columnOrdinal);
                    toReturn = new Response();
                }
                catch (Exception ee)
                {
                    toReturn = new Response(ee.Message);
                }
                return toReturn;
            }
          /// <summary>
          /// Adds a new column, given it's name and a location to place it.
          /// The first column is identified by 0, the ID increases by 1 for each column        
          /// </summary>
          /// <param name="email">Email of the user. Must be logged in</param>
          /// <param name="columnOrdinal">Location to place to column</param>
          /// <param name="Name">new Column name</param>
          /// <returns>A response object with a value set to the new Column, the response should contain a error message in case of an error</returns>
            public Response<Column> AddColumn(string email, int columnOrdinal, string Name)
            {
                Response<Column> toReturn;
                try
                {
                    BusinessLayer.BoardPackage.Column columnBusiness= boardController.AddColumn(email, columnOrdinal, Name);
                    Column columnService = new Column(columnBusiness);
                    toReturn = new Response<Column>(columnService);
                }
                catch (Exception ee)
                {
                    toReturn = new Response<Column>(ee.Message);
                }
                return toReturn;
            }
          /// <summary>
          /// Moves a column to the right, swapping it with the column wich is currently located there.
          /// The first column is identified by 0, the ID increases by 1 for each column        
          /// </summary>
          /// <param name="email">Email of the user. Must be logged in</param>
          /// <param name="columnOrdinal">Current location of the column</param>
          /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
            public Response<Column> MoveColumnRight(string email, int columnOrdinal)
            {
                Response<Column> toReturn;
                try
                {
                    boardController.MoveColumnRight(email, columnOrdinal);
                    BusinessLayer.BoardPackage.Column columnBusiness = boardController.GetColumn(email, columnOrdinal);
                    Column columnService = new Column(columnBusiness);
                    toReturn = new Response<Column>(columnService);
                }
                catch (Exception ee)
                {
                    toReturn = new Response<Column>(ee.Message);
                }
                return toReturn;
            }
          /// <summary>
          /// Moves a column to the left, swapping it with the column wich is currently located there.
          /// The first column is identified by 0, the ID increases by 1 for each column        
          /// </summary>
          /// <param name="email">Email of the user. Must be logged in</param>
          /// <param name="columnOrdinal">Current location of the column</param>
          /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
            public Response<Column> MoveColumnLeft(string email, int columnOrdinal)
            {
                Response<Column> toReturn;
                try
                {
                    boardController.MoveColumnLeft(email, columnOrdinal);
                    BusinessLayer.BoardPackage.Column columnBusiness = boardController.GetColumn(email, columnOrdinal);
                    Column columnService = new Column(columnBusiness);
                    toReturn = new Response<Column>(columnService);
                }
                catch (Exception ee)
                {
                    toReturn = new Response<Column>(ee.Message);
                }
                return toReturn;
            }
          public Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
          {
              Response toReturn;
              try
              {
                  boardController.AssignTask(email, columnOrdinal, taskId, emailAssignee);
                  toReturn = new Response();
              }
              catch (Exception ee)
              {
                  toReturn = new Response(ee.Message);
              }
              return toReturn;
          }
  
          /// <summary>        
          /// Loads the data. Intended be invoked only when the program starts
          /// </summary>
          /// <returns>A response object. The response should contain a error message in case of an error.</returns>
            public Response loadData()
                {
                Response toReturn;
                try
                {
                    boardController.LoadData();
                    toReturn = new Response();
                }
                catch (Exception ee)
                {
                    toReturn = new Response(ee.Message);
                }
                return toReturn;
            }
    
          public Response DeleteTask(string email, int columnOrdinal, int taskId)
          {
              Response toReturn;
              try
              {
                  boardController.DeleteTask(email, columnOrdinal, taskId);
                  toReturn = new Response();
              }
              catch (Exception ee)
              {
                  toReturn = new Response(ee.Message);
              }
              return toReturn;
          }
  
          public Response ChangeColumnName(string email, int columnOrdinal, string newName)

            {
                Response toReturn;
                try
                {
                    boardController.ChangeColumnName(email, columnOrdinal, newName);
                    toReturn = new Response();
                }
                catch (Exception ee)
                {
                    toReturn = new Response(ee.Message);
                }
                return toReturn;
            }
        }
    }
