

$("#menuButton").on("click", function () {

    $("#menu").modal("show");
})

$(".order-product-quantity").on("change", function () {
    var quantity = $(this).closest("input").val();
    var productId = $(this).closest("td").find("input").val();

    $.ajax({
        url: "UpdateOrder",
        type: "PUT",
        data: {
            Quantity: quantity,
            ProductId: productId
        },
        dataType: "json",
        success: function (response) {
            Console.log(response);
        }
    });

    update_total();

});

function update_total()
{
    var sum = 0.0;
    $("#order-table > tbody > tr").each(function () {
        var quantity = $(this).find("td input.order-product-quantity").val();
        var price = $(this).find("td input.product-price").val();

        var amount = (quantity * parseFloat(price));
        sum += amount;
    });

    $(".insert-total").text("Total: " + sum + " kr");
}

$(document).ready(function () {
    update_total();
});

$("input").keydown(function (event) {
    if (event.keyCode == 13) {
        event.preventDefault();
    }
})