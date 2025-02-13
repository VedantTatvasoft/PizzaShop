using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

[Table("role_permission_mapping")]
public partial class RolePermissionMapping
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("permissionid")]
    public int? Permissionid { get; set; }

    [Column("canview")]
    public bool? Canview { get; set; }

    [Column("canaddedit")]
    public bool? Canaddedit { get; set; }

    [Column("candelete")]
    public bool? Candelete { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Permissionid")]
    [InverseProperty("RolePermissionMappings")]
    public virtual Permission? Permission { get; set; }
}
