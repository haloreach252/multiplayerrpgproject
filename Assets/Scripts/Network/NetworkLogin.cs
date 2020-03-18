using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPressPCL;
using WordPressPCL.Models;
using UnityEngine;

public static class NetworkLogin {

	public static User user;
	public static bool loginSuccess = false;

	public static void Login(string username, string password, MainMenuController controller) {
		GetUser(username, password, controller).Wait(50);
	}

	public static void Register(string username, string password, string email, MainMenuController controller) {
		CreateUser(username, password, email, controller).Wait(50);
	}

	private static async Task GetUser(string username, string password, MainMenuController controller) {
		try {
			WordPressClient client = await GetClient(username, password);
			if (await client.IsValidJWToken()) {
				user = await client.Users.GetCurrentUser();
				if (user != null) {
					loginSuccess = true;
				} else {
					loginSuccess = false;
				}
				controller.Login(user, username, password);
			}
		} catch (Exception e) {
			Debug.LogError("Error: " + e.Message);
		}
	}

	private static async Task CreateUser(string username, string password, string email, MainMenuController controller) {
		try {
			WordPressClient client = await GetClient("userregister", "fPgkEb2ZLrW3BY#OA!xa#M9y");
			if (await client.IsValidJWToken()) {
				var createUser = new User() {
					UserName = username,
					Password = password,
					Email = email
				};
				var t = await client.Users.Create(createUser);
				if(t != null) {
					controller.Register(username, password);
				} else {
					Debug.LogError("Could not create user");
				}
			}
		} catch (Exception e) {
			Debug.LogError("Error: " + e.Message);
		}
	}

	private static async Task<WordPressClient> GetClient(string username, string password) {
		var client = new WordPressClient("http://miniversestudios.com/wp-json/");
		client.AuthMethod = AuthMethod.JWT;
		await client.RequestJWToken(username, password);
		return client;
	}

}
