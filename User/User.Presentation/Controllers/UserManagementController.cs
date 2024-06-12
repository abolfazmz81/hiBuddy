﻿using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using User.Application.UserManagement;

namespace User.Presentation.Controllers;

[ApiController]
[Produces("application/json")]
[Route("UserManagement")]
public class UserManagementController : ControllerBase
{
    
    private readonly HttpClient _httpClient;
    private readonly IDeleteUser _deleteUser;
    private string checkUrl = "http://localhost:5000/auth/CheckToken";

    public UserManagementController(HttpClient httpClient, IDeleteUser deleteUser)
    {
        _httpClient = httpClient;
        _deleteUser = deleteUser;
    }

    [HttpDelete("delete")]
    [Authorize]
    public async Task<ActionResult> DeleteUser([FromBody]String password)
    {
        if (password == "")
        {
            return BadRequest("no password provided");
        }
        string? token = HttpContext.Request.Headers.Authorization;
        token = token.Split(" ")[1];

        token = await CheckToken(token);
        if (token.Equals("invalid token provided"))
        {
            return BadRequest("invalid token provided");
        }

        if (token.Equals("wrong token"))
        {
            return BadRequest("wrong token");
        }

        String result = await _deleteUser.delete(token,password);
        if (result.Equals("not exists"))
        {
            return BadRequest("user with this token doesnt exists");
        }
        if (result.Equals("wrong password"))
        {
            return BadRequest("password is incorrect");
        }
        return Ok("");
    }
    // token check function
    private async Task<String> CheckToken(String token)
    {
        try
        {
            string jsonToken = JsonConvert.SerializeObject(token);
            // create http content to send
            HttpContent content = new StringContent(jsonToken, Encoding.UTF8, "application/json");
            // send request using post
            HttpResponseMessage response = await _httpClient.PostAsync(checkUrl, content);
            
            if (!response.IsSuccessStatusCode)
            {
                return "invalid token provided";
            }

            token = await response.Content.ReadAsStringAsync();
            token = token.Remove(0, 1).Remove(token.Length - 2);
            return token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "wrong token";
        }
    }
}