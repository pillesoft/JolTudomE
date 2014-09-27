﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JolTudomE_Api.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class JolTudomEEntities : DbContext
    {
        public JolTudomEEntities()
            : base("name=JolTudomEEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Sessions> Sessions { get; set; }
        public virtual DbSet<Person> Person { get; set; }
    
        public virtual int usp_AddNewUser(string username, string prefix, string lastname, string middlename, string firstname, string password, Nullable<byte> role)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var prefixParameter = prefix != null ?
                new ObjectParameter("prefix", prefix) :
                new ObjectParameter("prefix", typeof(string));
    
            var lastnameParameter = lastname != null ?
                new ObjectParameter("lastname", lastname) :
                new ObjectParameter("lastname", typeof(string));
    
            var middlenameParameter = middlename != null ?
                new ObjectParameter("middlename", middlename) :
                new ObjectParameter("middlename", typeof(string));
    
            var firstnameParameter = firstname != null ?
                new ObjectParameter("firstname", firstname) :
                new ObjectParameter("firstname", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            var roleParameter = role.HasValue ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(byte));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_AddNewUser", usernameParameter, prefixParameter, lastnameParameter, middlenameParameter, firstnameParameter, passwordParameter, roleParameter);
        }
    
        public virtual ObjectResult<usp_Authenticate_Result> usp_Authenticate(string username, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Authenticate_Result>("usp_Authenticate", usernameParameter, passwordParameter);
        }
    
        public virtual int usp_SessionsCleanup(Nullable<int> timeout)
        {
            var timeoutParameter = timeout.HasValue ?
                new ObjectParameter("timeout", timeout) :
                new ObjectParameter("timeout", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_SessionsCleanup", timeoutParameter);
        }
    
        public virtual int usp_CleanupTests(Nullable<int> timeout)
        {
            var timeoutParameter = timeout.HasValue ?
                new ObjectParameter("timeout", timeout) :
                new ObjectParameter("timeout", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_CleanupTests", timeoutParameter);
        }
    
        public virtual ObjectResult<usp_SearchUser_Result> usp_SearchUser(Nullable<int> roleid, string prefix, string firstname, string middlename, string lastname, string username)
        {
            var roleidParameter = roleid.HasValue ?
                new ObjectParameter("roleid", roleid) :
                new ObjectParameter("roleid", typeof(int));
    
            var prefixParameter = prefix != null ?
                new ObjectParameter("prefix", prefix) :
                new ObjectParameter("prefix", typeof(string));
    
            var firstnameParameter = firstname != null ?
                new ObjectParameter("firstname", firstname) :
                new ObjectParameter("firstname", typeof(string));
    
            var middlenameParameter = middlename != null ?
                new ObjectParameter("middlename", middlename) :
                new ObjectParameter("middlename", typeof(string));
    
            var lastnameParameter = lastname != null ?
                new ObjectParameter("lastname", lastname) :
                new ObjectParameter("lastname", typeof(string));
    
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_SearchUser_Result>("usp_SearchUser", roleidParameter, prefixParameter, firstnameParameter, middlenameParameter, lastnameParameter, usernameParameter);
        }
    
        public virtual ObjectResult<usp_GetCourses_Result> usp_GetCourses()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetCourses_Result>("usp_GetCourses");
        }
    
        public virtual ObjectResult<usp_GetTopics_Result> usp_GetTopics(Nullable<int> courseid)
        {
            var courseidParameter = courseid.HasValue ?
                new ObjectParameter("courseid", courseid) :
                new ObjectParameter("courseid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetTopics_Result>("usp_GetTopics", courseidParameter);
        }
    }
}
