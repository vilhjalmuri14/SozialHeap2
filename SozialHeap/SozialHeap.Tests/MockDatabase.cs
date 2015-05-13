using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Example.Data;
using FakeDbSet;
using System.Data.Entity;

using SozialHeap.Models;

namespace Sozialheap.Test
{
    /// <summary>
    /// This is an example of how we'd create a fake database by implementing the 
    /// same interface that the BookeStoreEntities class implements.
    /// </summary>
    public class MockDatabase : IAppDataContext
    {
        /// <summary>
        /// Sets up the fake database.
        /// </summary>
        public MockDatabase()
        {
            // We're setting our DbSets to be InMemoryDbSets rather than using SQL Server.
            this.C__MigrationHistory = new InMemoryDbSet<C__MigrationHistory>();
            this.Answers = new InMemoryDbSet<Answer>();
            this.AspNetRoles = new InMemoryDbSet<AspNetRole>();
            this.AspNetUserClaims = new InMemoryDbSet<AspNetUserClaim>();
            this.AspNetUserLogins = new InMemoryDbSet<AspNetUserLogin>();
            this.AspNetUsers = new InMemoryDbSet<AspNetUser>();
            this.Groups = new InMemoryDbSet<Group>();
            this.PostCategories = new InMemoryDbSet<PostCategory>();
            this.Posts = new InMemoryDbSet<Post>();
            this.Users = new InMemoryDbSet<User>();
        }

        // Ég bætti þessu við
        public IDbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public IDbSet<Answer> Answers { get; set; }
        public IDbSet<AspNetRole> AspNetRoles { get; set; }
        public IDbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public IDbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public IDbSet<AspNetUser> AspNetUsers { get; set; }
        public IDbSet<Group> Groups { get; set; }
        public IDbSet<PostCategory> PostCategories { get; set; }
        public IDbSet<Post> Posts { get; set; }
        public IDbSet<User> Users { get; set; }

        public int SaveChanges()
        {
            // Pretend that each entity gets a database id when we hit save.
            int changes = 0;
            //changes += DbSetHelper.IncrementPrimaryKey<Author>(x => x.AuthorId, this.Authors);
            //changes += DbSetHelper.IncrementPrimaryKey<Book>(x => x.BookId, this.Books);

            return changes;
        }

        public void Dispose()
        {
            // Do nothing!
        }
    }
}

