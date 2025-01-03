﻿using System;

namespace gitserverdotnet.Data.Update
{
    public class UsernamesToLower : IUpdateScript
    {
        public string Command
        {
            get 
            {
                return @"
                    UPDATE [User] SET Username = lower(Username);
                ";
            }
        }

        public string Precondition
        {
            get { return null; }
        }

        public void CodeAction(gitserverdotnetContext context) { }

    }
}