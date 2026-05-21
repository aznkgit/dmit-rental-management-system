using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSSystem.ViewModels
{
    /// <summary>
    /// Class RentalPaymentView.
    /// </summary>
    internal class RentalPaymentView
    {
        /// <summary>
        /// Gets or sets the payment identifier.
        /// </summary>
        /// <value>The payment identifier.</value>
        public int PaymentID { get; set; }
        /// <summary>
        /// Gets or sets the rental identifier.
        /// </summary>
        /// <value>The rental identifier.</value>
        public int RentalID { get; set; }
        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>The month.</value>
        public byte Month { get; set; }
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        public int Year { get; set; }
        /// <summary>
        /// Gets or sets the amount paid.
        /// </summary>
        /// <value>The amount paid.</value>
        public decimal AmountPaid { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [paid in full].
        /// </summary>
        /// <value><c>true</c> if [paid in full]; otherwise, <c>false</c>.</value>
        public bool PaidInFull { get; set; }
        public bool RemoveFromViewFlag { get; set; }
    }
}
