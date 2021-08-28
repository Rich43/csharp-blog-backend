using System.ComponentModel.DataAnnotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace blog.dto {
    public class User : Identifiable {
        public override int Id {get; set;}
        [Attr(Capabilities = AttrCapabilities.AllowSort | AttrCapabilities.AllowFilter), Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Attr(Capabilities = AttrCapabilities.AllowSort | AttrCapabilities.AllowFilter), Required]
        public string EMail { get; set; }
    }
}
