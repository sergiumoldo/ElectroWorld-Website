﻿using Newtonsoft.Json;
using System;

namespace Pipelights.Database.Models
{
    public abstract class Entity
    {
        /// <summary>
        /// Default document entity identifier
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Data time to live
        /// </summary>
        [JsonProperty(PropertyName = "ttl")]
        public int Ttl { get; set; }

        /// <summary>
        /// Entity
        /// </summary>
        /// <param name="generateId">Generate id</param>
        public Entity(bool generateId)
        {
            SetDefaultTimeToLive();

            if (generateId)
                this.Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Set a default data time to live
        /// </summary>
        public virtual void SetDefaultTimeToLive()
        {
            Ttl = -1;
        }
    }
}
