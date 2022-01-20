using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.DataAccess.Model;

namespace Silvercrest.Web.Areas.Client.Views.Shared
{
    public class URLParameters
    {
        public int contactId { get; set; }
        public int? entityId { get; set; }
        public bool? isGroup { get; set; }
        public bool? isClientGroup { get; set; }
        public int? groupEntityId { get; set; }
        public bool? groupIsClientGroup { get; set; }
        public string contactFullName { get; set; }
        private static readonly SLVR_DEVEntities _context = new SLVR_DEVEntities();
        private static ContactRepository _contactRepository = new ContactRepository(_context);


        public URLParameters(int contactId)
        {
            this.contactId = contactId;
            this.entityId = null;
            this.isGroup = null;
            this.isClientGroup = null;
            this.groupEntityId = null;
            this.groupIsClientGroup = null;

            contactFullName = GetContactFullName(contactId);

        }

        public static string GetContactFullName(int contactId)
        {
            return  _contactRepository.GetFullNameByContactId(contactId);

        }

        public void resolveHoldingsParameters(int? entityId, bool? isGroup, bool? isClientGroup)
        {
            if(entityId== null)
            {
                //If entityId is null, then we just use that since it was used last click
                if(this.entityId != null)
                {

                } //If entityId is also null from last visit, lets us the groupIsClientGroup from last visit
                else if(this.groupEntityId != null)
                {
                    this.entityId = this.groupEntityId;
                    this.isGroup = true;
                    this.isClientGroup = this.groupIsClientGroup;
                }
            }
            else
            {
                this.entityId = entityId;
                this.isGroup = isGroup;
                this.isClientGroup = isClientGroup;

                if(isGroup == true)
                {
                    this.groupEntityId = entityId;
                    this.groupIsClientGroup = isClientGroup;
                }
            }

        }


        public void resolveHomeParameters(int? gEI, bool? gICG, int? eI, bool? iG, bool? iCG)
        {
            if (gEI != null)
            {
                this.groupEntityId = gEI;
                this.groupIsClientGroup = gICG;
                if(eI == null)
                {
                    this.entityId = null;
                    this.isGroup = null;
                    this.isClientGroup = null;
                }
                else
                {
                    this.entityId = eI;
                    this.isGroup = false;
                    this.isClientGroup = false;
                }

                return;
            }
            else if((eI != null) &&(iG == true))
            {
                this.groupEntityId = eI;
                this.groupIsClientGroup = iCG;
                this.entityId = null;
                this.isGroup = null;
                this.isClientGroup = null;
                return;
            }
            else
            {   //Leave value to what is was the last time it was set
//                this.groupEntityId = null;
//                this.groupIsClientGroup = null;
                  
                  //We leave group details as is,but we need to fix the entity details otherwise will fail
                if(this.isGroup == true)
                {
                    this.entityId = null;
                    this.isGroup = null;
                    this.isClientGroup = null;
                }
                return;
            }

            if (eI == null)  //Leave as it was
            {
//                this.entityId = eI;
 //               this.isGroup = null;
 //               this.isClientGroup = null;
            }
            else
            {
                this.entityId = eI;
                this.isGroup = iG;
                this.isClientGroup = iCG;
            }
        }
    }
}