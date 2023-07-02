var BillType = ""
var Vendor=""
var cardNumber = ""
var validity = ""
var cvv = ""
var amount = ""
var accountNumber = ""
var password = ""
var walletBalance = 100000

$(document).ready(() => {

  

    $("#select-vendor-content").hide();
    $("#pay-bill-content").hide();
    $("#select-bill-main").hide();


    
    $("#a-select-bill").addClass("active");
    $("#a-select-vendor").addClass('disabled')
    $("#a-pay").addClass('disabled')

    $("#creditcard").hide();
    $("#internet-banking-div").hide();
    $("#paytm-div").hide();
    $("#wallet-div").hide();

    $("#redirecting").hide();


    

    

    
   

    var button = document.getElementById("bill-type-submit")
    var button2 = document.getElementById("vendor-type-submit")
    var checkoutButton = document.getElementById("checkout-submit")
    var input = document.getElementById("amount")

    var checkoutButton_2 = document.getElementById("checkout-submit-2")
    var checkoutButton_3 = document.getElementById("checkout-submit-3")
    var checkoutButton_4 = document.getElementById("checkout-submit-4")

   

    button.disabled = true;
    button2.disabled = true;
    input.disabled = true;
    checkoutButton.disabled = true;
    checkoutButton_2.disabled = true;
    checkoutButton_3.disabled = true;

    checkoutButton_4.disabled = true;

 
    $("#a-select-bill").click(
         ()=> {
            //toggleClass() switches the active class 
            $("#a-select-bill").addClass("active");
            $("#a-select-vendor").removeClass("active");
            $("#a-pay").removeClass("active");


            $("#select-bill-content").show();
            $("#select-vendor-content").hide();
            $("#pay-bill-content").hide();
        }
    ); 


    $("#a-select-vendor").click(
        () => {
            //toggleClass() switches the active class 
            $("#a-select-vendor").addClass("active");
            $("#a-select-bill").removeClass("active");
            $("#a-pay").removeClass("active");

            $("#select-vendor-content").show();
            $("#select-bill-content").hide();
            $("#pay-bill-content").hide();


        }
    ); 

    $("#a-pay").click(
        () => {
            //toggleClass() switches the active class 
            $("#a-pay").addClass("active");
            $("#a-select-bill").removeClass("active");
            $("#a-select-vendor").removeClass("active");

           
            $("#select-bill-content").hide();
            $("#pay-bill-content").show();


        }
    ); 


    $("#bill-types").change(function () {
        const selectedValue = $(this).children("option:selected").val();
        if (selectedValue !== "") {
            BillType = selectedValue;
            button.disabled = false;
        } else {
            button.disabled = true;
        }

        $("#already-paid").remove();


         Vendor = ""
         cardNumber = ""
         validity = ""
         cvv = ""
        amount = ""
        var input = document.getElementById("amount").value=""
       
    });


    $("#vendor-list").change(function () {
        const selectedValue = $(this).children("option:selected").val();
        Vendor = selectedValue;
        if (amount!=="") {
            button2.disabled = false;
        } 
    });
    $("#amount").change(function () {
        const selectedValue = $(this).val();
        amount = selectedValue;
        if (Vendor !== "") {
            button2.disabled = false;
            if (amount <= walletBalance) {
                var checkoutButton_4 = document.getElementById("checkout-submit-4")
                checkoutButton_4.disabled = false;
            }
        }

    });

    $("#card-number").change(function () {
        const selectedValue = $(this).val();
        cardNumber = selectedValue;
        if (validity !== "" && cvv !== "") {
            checkoutButton.disabled = false;
        }
    });
    $("#card-date").change(function () {
        const selectedValue = $(this).val();
        validity = selectedValue;
        if (cardNumber !== "" && cvv !== "") {
            checkoutButton.disabled = false;
        }
    });

    $("#card-cvv").change(function () {
        const selectedValue = $(this).val();
        cvv = selectedValue;
        if (validity !== "" && cvv !== "") {
            checkoutButton.disabled = false;
        }
    });




    $("#ib-number").change(function () {
        const selectedValue = $(this).val();
        accountNumber = selectedValue;
        if (accountNumber !== "" && password!=="") {
            checkoutButton_2.disabled = false;
        }
    });


    $("#ib-password").change(function () {
        const selectedValue = $(this).val();
        password = selectedValue;
        if (accountNumber !== "" && password !== "") {
            checkoutButton_2.disabled = false;
        }
    });


    $("#paytm-numberd").change(function () {
        const selectedValue = $(this).val();
       
        if (selectedValue === "") {
            var checkoutButton_3 = document.getElementById("checkout-submit-3")
            var checkoutButton_4 = document.getElementById("checkout-submit-4")

            checkoutButton_3.disabled = true;
            checkoutButton_4.disabled = true;
        }
    });


    






})
const fetchBalance = () => {
    console.log("Test", document.getElementById("paytm-number").value);
    if (document.getElementById("paytm-number").value !== "") {
        $("#pay-bal").remove();

        $("#paytm-balance").append("<p id='pay-bal'>You have Rs. 787823 in your wallet</p>");

        var checkoutButton_3 = document.getElementById("checkout-submit-3")
        var checkoutButton_4 = document.getElementById("checkout-submit-4")

        checkoutButton_3.disabled = false;
        checkoutButton_4.disabled = false;

    }
}


const onBillTypeSubmit = () => {

    $("#a-select-vendor").addClass("active");
    $("#a-select-bill").removeClass("active");
    $("#a-pay").removeClass("active");

    $("#select-vendor-content").show();
    $("#select-bill-content").hide();
    $("#pay-bill-content").hide();

    $("#a-select-vendor").removeClass('disabled')
    getVendorData();
}

const onVendorTypeSubmit = () => {
    $("#select-vendor-content").hide();
    $("#select-bill-content").hide();
    $("#pay-bill-content").show();

    $("#a-select-vendor").removeClass("active");
    $("#a-select-bill").removeClass("active");
    $("#a-pay").addClass("active");

}
const getVendorData = () => {

    const user_id = $('#myHiddenVar').val()
  
        $.ajax({
            url: `/Customer/GetPaymentDetails?Category=${BillType}&userId=${user_id}`,   //url of {controller/action}
            type: "get",  //type of request (http verb)
            success: (response) => {
                 // on successful response
               
                console.log(response)
                if ("Vendors" in response) {
                    try {
                        $('#vendor-list').empty();
                        $('#vendor-list').append(`<option  value="">Select</option>`)
                        response["Vendors"].map(vendor => {

                            $('#vendor-list').append(`<option  value="${vendor.UserId}">${vendor.FirstName + " " + vendor.LastName}</option>`);
                        })
                    } catch (e) {

                    }
                   

                }
                if (response.Paid) {
                    $("#select-bill-main").hide();

                    $("#select-vendor-content").append(`<div id="already-paid"><br/><br/><h4>You've already paid for this month</h4></div>`);
                    

                } else {

                    $("#select-bill-main").show();

                }
                if (response.Amount === 0) {
                   
                    var input = document.getElementById("amount")
                    input.disabled = false;
                } else {
                    var input = document.getElementById("amount")
                    input.disabled = true;
                    input.value = response.Amount;
                    amount = response.Amount;
                    
                }
                if (amount <= walletBalance) {

                    var checkoutButton_3 = document.getElementById("checkout-submit-4")
                    checkoutButton_3.disabled = false;

                    var checkoutButton_4 = document.getElementById("checkout-submit-4")
                    checkoutButton_4.disabled = false;
                }
                
                 
            },
            error: (xhr) => {
                //on error response
            }
        });

}


const selectPaymentMethod = (type) => {

    switch (type) {

        case 'card':
            $("#card").addClass("payment-div-active");
            $("#card").removeClass("payment-div-inactive");
            $("#ib").removeClass("payment-div-active");
            $("#paytm").removeClass("payment-div-active");
            $("#gpay").removeClass("payment-div-active");
            $("#wallet").removeClass("payment-div-active");
            showCardMethod()
        
            $("#internet-banking-div").hide();
            $("#paytm-div").hide();
            $("#wallet-div").hide();


            break;

        case 'ib':
            $("#ib").addClass("payment-div-active");
            $("#ib").removeClass("payment-div-inactive");
            $("#card").removeClass("payment-div-active");
            $("#paytm").removeClass("payment-div-active");
            $("#gpay").removeClass("payment-div-active");
            $("#wallet").removeClass("payment-div-active");
            $("#creditcard").hide();
           
            $("#paytm-div").hide();
            $("#wallet-div").hide();
            $("#internet-banking-div").show();



            break;

        case 'paytm':
            $("#paytm").addClass("payment-div-active");
            $("#paytm").removeClass("payment-div-inactive");
            $("#ib").removeClass("payment-div-active");
            $("#card").removeClass("payment-div-active");
            $("#gpay").removeClass("payment-div-active");
            $("#wallet").removeClass("payment-div-active");
            $("#creditcard").hide();
            $("#internet-banking-div").hide();
            $("#wallet-div").hide();
            $("#paytm-div").show();



            break;

        case 'gpay':
            $("#gpay").addClass("payment-div-active");
            $("#gpay").removeClass("payment-div-inactive");
            $("#ib").removeClass("payment-div-active");
            $("#paytm").removeClass("payment-div-active");
            $("#card").removeClass("payment-div-active");
            $("#wallet").removeClass("payment-div-active");
            $("#creditcard").hide();
            $("#internet-banking-div").hide();
            $("#wallet-div").hide();
            $("#paytm-div").show();




            break;

        case 'wallet':
            $("#wallet").addClass("payment-div-active");
            $("#wallet").removeClass("payment-div-inactive");
            $("#ib").removeClass("payment-div-active");
            $("#paytm").removeClass("payment-div-active");
            $("#gpay").removeClass("payment-div-active");
            $("#card").removeClass("payment-div-active");
      
            $("#creditcard").hide();
            $("#internet-banking-div").hide();
            $("#paytm-div").hide();
            $("#wallet-div").show();


            break;


        default:
            console.log("Clciked");
    }

    console.log(type)
}

const showCardMethod = () => {
    $("#creditcard").show();
}


const savePaymentDetails = (billtype) => {

    console.log(BillType,Vendor)
    const user_id = $('#myHiddenVar').val()
 

    $.ajax({
        url: "/Customer/SavePaymentDetails/",
        type: "POST",
        data: {
            "UserId":Number(user_id),
            "VendorId": Vendor,
            "BillType": BillType,
            "BillAmount": amount,
            "Success":true
        },//url of {controller/action}
        //type of request (http verb)
        success: (response) => {
            
            if (response === "Success") {

                $("#main-container").empty();

                $("#main-container").append(`<div id="redirecting"><br/><br/><h6>You've paid successfuly! Redirecting to payments...</h6></div>`);


                setTimeout( () =>{
                    window.location = "/Customer/PaymentHistory";
                }, 4000);


            } else {
                toastr.error("Something went wrong...")
            }
        },
        error: (xhr) => {
            toastr.error("Something went wrong...")

            //on error response
        }
    });
}