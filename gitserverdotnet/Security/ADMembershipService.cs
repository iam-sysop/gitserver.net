﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Data.Entity.Core;
using System.Diagnostics;
using gitserverdotnet.Data;
using gitserverdotnet.Models;
using System.Web.Security;
using System.Security.Principal;
using gitserverdotnet.Configuration;
using gitserverdotnet.Helpers;
using Serilog;

namespace gitserverdotnet.Security
{
    public class ADMembershipService : IMembershipService
    {
        public bool IsReadOnly()
        {
            return true;
        }

        public ValidationResult ValidateUser(string username, string password)
        {
            ValidationResult result = ValidationResult.Failure;

            if (String.IsNullOrEmpty(username)) throw new ArgumentException("Value cannot be null or empty", "username");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty", "password");

            try
            {
                if (ADHelper.ValidateUser(username, password))
                {
                    UserPrincipal user = ADHelper.GetUserPrincipal(username);
                    GroupPrincipal group;
                    PrincipalContext pc = ADHelper.GetMembersGroup(out group);

                    if (group == null)
                        result = ValidationResult.Failure;

                    if (user != null)
                    {
                        if (!ADHelper.GetGroupMembers(group).Contains(user))
                        {
                            result = ValidationResult.NotAuthorized;
                        }
                        else
                        {
                            result = ValidationResult.Success;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex, "AD.ValidateUser Exception: ");
                result = ValidationResult.Failure;
            }

            return result;
        }

        public bool CreateUser(string username, string password, string givenName, string surname, string email, Guid id)
        {
            return false;
        }

        public bool CreateUser(string username, string password, string givenName, string surname, string email)
        {
            return false;
        }

        public IList<UserModel> GetAllUsers()
        {
            var users = ADBackend.Instance.Users.ToList();
            return users;
        }

        public UserModel GetUserModel(string username)
        {
            UserPrincipal upc = ADHelper.GetUserPrincipal(username);

            return ADBackend.Instance.Users.FirstOrDefault(n => n.Id == upc.Guid.Value);

            throw new ArgumentException("User was not found with username: " + username);
        }

        public UserModel GetUserModel(Guid id)
        {
            return ADBackend.Instance.Users[id];
        }

        private static bool UsernameContainsDomain(string username)
        {
            return String.IsNullOrEmpty(username) && !string.IsNullOrEmpty(username.GetDomain());
        }

        public void UpdateUser(Guid id, string username, string givenName, string surname, string email, string password)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public string GenerateResetToken(string username)
        {
            throw new NotImplementedException();
        }
    }
}
