

$(".test").change(function () {
    let quantity = $(".order-product-quantity").val();
    let price = $(".product-price").html();

    let total = parseFloat(quantity) * parseFloat(price);
    $(this).children(".insert-total").html(total);
    $(".insert-total").html(total);
    $.ajax({
        type: "POST",
        url: "/Order/UpdateOrder",
        data: JSON.stringify(quantity),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r) {
                alert("Order quantity updated.");
            } else {
                alert("Order update failed.");
            }
        }
    });
});