using Kvite.Web.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Kvite.Data;
using Kvite.Web.Domain;
using Kvite.Web.Services.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using Kvite.Web.Enums;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Kvite.Web.Services
{
    public class ContactsService : BaseService, IContactsService
    {
        private IUsersService _usersService;
        private String photoUrlDefault = @"Content\Views\Contacts\ContactDefault.png";

        public ContactsService(IUsersService userService)
        {
            _usersService = userService;
        }

        private SqlParameterCollection CreateParams(AddContactsRequest model, SqlParameterCollection parameters)
        {
            parameters.AddWithValue("@firstName", model.FirstName);
            parameters.AddWithValue("@lastName", model.LastName ?? (object)DBNull.Value);
            parameters.AddWithValue("@mobilePhoneNumber", model.MobilePhoneNumber ?? (object)DBNull.Value);
            parameters.AddWithValue("@email", model.Email ?? (object)DBNull.Value);
            if (model.SourceId == null)
            {
                parameters.AddWithValue("@sourceId", DBNull.Value);
            }
            else
            {
                parameters.AddWithValue("@sourceId", model.SourceId);
            }
            if (model.SourceType == null)
            {
                parameters.AddWithValue("@sourceType", DBNull.Value);
            }
            else
            {
                parameters.AddWithValue("@sourceType", model.SourceType);
            }
            parameters.AddWithValue("@photoUrl", model.PhotoUrl ?? photoUrlDefault);

            parameters.AddWithValue("@contactUserId", DBNull.Value);
            parameters.AddWithValue("@ownerUserId", _usersService.GetCurrentUserId());
            return parameters;
        }
        public int Create(AddContactsRequest model)
        {
            int id = 0;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Contacts_Insert",
                inputParamMapper: delegate (SqlParameterCollection parameters)
                {
                    parameters = CreateParams(model, parameters);
                    SqlParameter idParameter = new SqlParameter("@id", SqlDbType.Int);
                    idParameter.Direction = ParameterDirection.Output;
                    parameters.Add(idParameter);
                },

                returnParameters: delegate (SqlParameterCollection parameters)
                {
                    int.TryParse(parameters["@id"].Value.ToString(), out id);
                }

            );
            return id;
        }

        public void Update(UpdateContactsRequest model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Contacts_Update",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection = CreateParams(model, paramCollection);
                    paramCollection.AddWithValue("@id", model.Id);
                });
        }

        public List<Contact> GetAllContacts()
        {
            List<Contact> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Contacts_SelectAll"
                , map: delegate (IDataReader reader, short set)
                {
                    Contact contact = ReadSqlColumn(reader);
                    if (contact.LastName == null)
                    {
                        contact.LastName = " ";
                    }
                    if (list == null)
                    {
                        list = new List<Contact>();
                    }

                    list.Add(contact);

                });
            return list;
        }

        public List<Contact> GetContactsOwnedByCurrentUserId()
        {
            List<Contact> list = null;

            DataProvider.ExecuteCmd(
                GetConnection
                , "dbo.Contacts_SelectByOwnerUserId"
                , inputParamMapper: delegate (SqlParameterCollection parameter)
                {
                    parameter.AddWithValue("@ownerUserId", _usersService.GetCurrentUserId());
                }
                , map: delegate (IDataReader reader, short set)
                {
                    if (list == null)
                    {
                        list = new List<Contact>();
                    }
                    Contact contact = ReadSqlColumn(reader);
                    list.Add(contact);
                });
            return list;
        }

        public List<Contact> GetContactsByEventCreatorUserId(string userId)
        {
            List<Contact> list = null;

            DataProvider.ExecuteCmd(
                GetConnection
                , "dbo.Contacts_SelectByOwnerUserId"
                , inputParamMapper: delegate (SqlParameterCollection parameter)
                {
                    parameter.AddWithValue("@OwnerUserId", userId);
                }
                , map: delegate (IDataReader reader, short set)
                {
                    if (list == null)
                    {
                        list = new List<Contact>();
                    }
                    Contact contact = ReadSqlColumn(reader);
                    list.Add(contact);
                });
            return list;
        }

        public void DeleteById(int contactId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Contacts_DeleteById",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@id", contactId);
                });
        }

        public Contact EditById(int contactId)
        {
            Contact contact = new Contact();
            DataProvider.ExecuteCmd(GetConnection, "dbo.Contacts_SelectById"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@id", contactId);
                }, map: delegate (IDataReader reader, short set)
                {
                    contact = ReadSqlColumn(reader);

                });
            return contact;
        }

        public Contact ReadSqlColumn(IDataReader reader)
        {
            Contact x = new Contact();
            int startingIndex = 0;

            x.Id = reader.GetSafeInt32(startingIndex++);
            x.FirstName = reader.GetSafeString(startingIndex++);
            x.LastName = reader.GetSafeString(startingIndex++);
            x.MobilePhoneNumber = reader.GetSafeInt64Nullable(startingIndex++);
            x.Email = reader.GetSafeString(startingIndex++);
            x.PhotoUrl = reader.GetSafeString(startingIndex++);
            x.SourceId = reader.GetSafeString(startingIndex++);
            x.SourceType = reader.GetSafeEnum<ContactSourceKinds>(startingIndex++);
            x.ContactUserId = reader.GetSafeString(startingIndex++);
            x.OwnerUserId = reader.GetSafeString(startingIndex++);
            x.IsDeleted = reader.GetSafeBool(startingIndex++);
            x.DateCreated = reader.GetSafeDateTime(startingIndex++);
            x.DateModified = reader.GetSafeDateTime(startingIndex++);

            return x;
        }

    }
}