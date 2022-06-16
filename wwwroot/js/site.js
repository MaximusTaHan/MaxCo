

$(".test").change(function () {
    let quantity = $(".order-product-quantity").val();
    let price = $(".product-price").html();

    let total = parseFloat(quantity) * parseFloat(price);
    $(this).children(".insert-total").html(total);
    $(".insert-total").html(total);
});