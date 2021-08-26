using blog.dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace blog.Database {
    public DBSet<BlogEntry> BlogEntries { get; set; };
    
    public class BlogDBContext : DbContext {
        public BlogDBContext() {

        }
    }
}