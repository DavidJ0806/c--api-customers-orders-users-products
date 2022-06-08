using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    /// <summary>
    /// CustomerAddress class to represent customers addresses
    /// </summary>
    public class CustomerAddress
    {

        [Key, ForeignKey("CustomerAddressId")]
        public int Id { get; set; }
        [Required]
        public string? Street { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        [RegularExpression("^(A[LKSZRAEP]|C[AOT]|D[EC]|F[LM]|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ADEHINOPST]|N[CDEHJMVY]|O[HKR]|P[ARW]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY])$")]
        public string? State { get; set; }
        [Required]
        [RegularExpression("^[0-9]{5}$|^[0-9]{5}\\-[0-9]{4}$")]
        public string? ZipCode { get; set; }
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
    }
}