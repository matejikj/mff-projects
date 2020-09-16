$(document).ready(function(){

    $('.td-save').hide();
    $('.td-cancel').hide();
    $('.td-text').hide();
    $('.td-edit').show();
    $('.td-delete').show();

    $('.btn-swap:last').hide();  

    $("button").click(function() {
        var myClass = $(this).attr("class");
        

        if ( myClass == 'btn btn-cancel' ) {
            $('.td-edit').show();
            $('.td-delete').show();
            $('.td-swap').show();

            $('.td-text').hide();
            $('.td-save').hide();
            $('.td-cancel').hide();
        }

    });
     

});

function deleteItem(id) {
    $.ajax({

        type:'get',
        url:'/AJAX/delete',
        data:{delete_id:id},
        success:function(data){
           $('#row'+id).remove();
           var n = $('.btn-swap').length;
           if ( n < 2 ){
            $('.btn-swap').hide();
           }
        }
   });


   

}

function editItem(id) {
    $('.td-edit').hide();
    $('.td-delete').hide();
    $('.td-swap').hide();
    $('.td-text').show();
    $('.input-text').hide();


    $('.td-save').show();
    $('.td-cancel').show();
    $('.btn-save').hide();
    $('.btn-cancel').hide();

    $('#cancel'+id).show();
    $('#input'+id).show();
    $('#save'+id).show();
}

function saveItem(id) {

    var amount = $('#input'+id)[0].value;

    $.ajax({
        type:'post',
        url:'/AJAX/update',
        data:{
            id:id,
            amount:amount
        },
        success:function(data){
           $('#text'+id)[0].innerHTML = amount;

        }
   });

   $('.td-edit').show();
   $('.td-delete').show();
   $('.td-swap').show();

   $('.td-text').hide();
   $('.td-save').hide();
   $('.td-cancel').hide();
}

function swapItem(first, second) {

    $.ajax({
        type:'get',
        url:'/list/swap',
        data:{
            first_id: first,
            second_id: second
        },
        success:function(data){
            
        }
   });
}