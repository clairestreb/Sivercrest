$(document).ready(function () {
    $(".btn-download").click(function () {
        $(this).parent().find(".btnSlider").slideToggle();
        return false;
    });
    $(".btn-pdf, .btn-exl").click(function () {
        $(this).parent().parent().find(".btnSlider").slideToggle();
        return false;
    });

    $("#selector .xtoggle").click(function () {
        $(this).toggleClass("temp-active")
        return false;
    });

    $("#selector2 .xtoggle").click(function () {
        $(this).toggleClass("active")
        return false;
    });

    $(".a1").click(function () {
        $(this).parent().parent().find('.a2').toggleClass("active");
        $(this).toggleClass("active");
        return false;
    });
    $(".a2").click(function () {
        $(this).parent().parent().find('.a1').toggleClass("active");
        $(this).toggleClass("active");
        return false;
    });


    $('#email-popup .state1, #email-popup .state2 a').click(function () {
        $('#email-popup').toggleClass('show1 show2');
        return false;
    });

    $('#submit').click(function () {
        if ($("#name").val() != "" && $("#message").val() != "") {
            $('#email-popup').removeClass('show1 show2').addClass('show3');
            setTimeout(function () {
                $('#email-popup').removeClass('show3').addClass('show1');
            }, 5000);
            return false;
        }
        else {
            $('#messageError').html("Please fill out all the fields");
            return false;
        }
        return false;
    });

    $('#closePopup').click(function () {
        $('#messageError').html("");
        $('#email-popup').removeClass('show2').addClass('show1');
        return false;
    });

    $('#password').on("change keyup paste", function () {
        $('#result').html(checkStrength($('#password').val()))
    });
    function checkStrength(password) {
        var strength = 0
        if (password.length < 8) {
            $('#result').removeClass()
            $('#result').addClass('short')
            return 'The password is too short! The minimal length of password is 8 characters!';
        }
        if (!password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/))
            return 'Must contain at least one uppercase and one lowercase character';
        if (password.match(/([a-zA-Z])/) && !password.match(/([0-9])/))
            return 'Must contain at least one number';

        return '';
    }

    $('#form').submit(function () {
        if ($("#firstQuest").val() == null || $("#secondQuest").val() == null ||
            $("#thirdQuest").val() == null || $("#firstAnswer").val() == "" ||
            $("#secondAnswer").val() == "" || $("#thirdAnswer").val() == "") {
            $("#validationMessage").children().children().html("Please fill out all the fields").css("display", "block");
            window.scrollTo(0, 0);
            return false;
        }
        if (checkStrength($('#password').val()) != '') {
            $("#validationMessage").children().children().html("Please check your password").css("display", "block");
            window.scrollTo(0, 0);
            return false;
        }
        if ($('#password').val() !== $('#passwordcheck').val()) {
            $("#validationMessage").children().children().html("Password and confirmation password must be the same").css("display", "block");
            window.scrollTo(0, 0);
            return false;
        }
        if ($("#passwordOld").val() == "") {
            $("#validationMessage").children().children().html("Please check old password").css("display", "block");
            window.scrollTo(0, 0);
            return false
        }
    });
    $('#form1').submit(function () {
        if ($("#firstAnswer").val() == "" ||
            $("#secondAnswer").val() == "" || $("#thirdAnswer").val() == "") {
            $("#validationMessage").children().children().html("Please, fill out all fields for secret questions answers ").css("display", "block");
            window.scrollTo(0, 0);
            return false;
        }
        if ($('#password').val() == "" || $('#passwordcheck').val() == "") {
            $("#validationMessage").children().children().html("Please, fill out all fields ").css("display", "block");
            window.scrollTo(0, 0);
            return false;
        }
    });
    $('#form1').find('#password').on('change keyup paste', function () {
        var passwordError = checkStrength($('#password').val());
        if (passwordError != '') {
            $("#validationMessage").children().children().html(passwordError).css("display", "block");
            return false;
        }
        else {
            $("#validationMessage").children().children().html("").css("display", "block");
        }
    });

    $('#form1').find('#passwordcheck').on('change keyup paste', function () {
        if ($('#password').val().localeCompare($('#passwordcheck').val()) != 0) {
            $("#validationMessage").children().children().html("Password and confirmation password must be the same").css("display", "block");
            return false;
        }
        else {
            $("#validationMessage").children().children().html("").css("display", "block");
        }
    });

    $('#formPassword').find('#password').on('change keyup paste', function () {
        var passwordError = checkStrength($('#password').val());
        if (passwordError != '') {
            $("#validationMessagePassword").children().children().html(passwordError).css("display", "block");
            return false;
        }
        else {
            $("#validationMessagePassword").children().children().html("").css("display", "block");
        }
    });

    $(window).scroll(function () {
        var height = $(window).scrollTop();
        var windowWidth = $(window).width();
        if (height > 167 + 1) { //167 - height header image
            $('#navbar-desctop').css('top', 0);
            $('#navbar-desctop').css('position', 'fixed');
            $('#navbar-desctop').css('z-index', '100');
            $('#navbar-desctop').css('width', windowWidth + 'px');
        } else {
            $('#navbar-desctop').css('top', '');
            $('#navbar-desctop').css('position', '');
            $('#navbar-desctop').css('margin-left', '');
            $('#navbar-desctop').css('z-index', '');
            $('#navbar-desctop').css('width', '');
        }
    });

    $(window).resize(function () {
        var windowWidth = $(window).width();
            $('nav.desktop.hidden-xs').css('width', windowWidth + 'px');
    });
    


    //$('#form1').find('#firstAnswer').on('change keyup paste', function () {
    //    if ($('#firstAnswer').val().length == 0
    //        || $('#secondAnswer').val().length == 0
    //        || $('#thirdAnswer').val().length == 0
    //        ) {
    //        var text = "You have  " + getAttetmps() + " attempts remaining";
    //        $("#validationMessage").children().children().html(text).css("display", "block");
    //        return false;
    //    }
    //    else {
    //        $("#validationMessage").children().children().html("").css("display", "block");
    //    }
    //});

    //$('#form1').find('#secondAnswer').on('change keyup paste', function () {
    //    if ($('#firstAnswer').val().length == 0
    //        || $('#secondAnswer').val().length == 0
    //        || $('#thirdAnswer').val().length == 0
    //        ) {
    //        var text = "You have  " + getAttetmps() + " attempts remaining";
    //        $("#validationMessage").children().children().html(text).css("display", "block");
    //        return false;
    //    }
    //    else {
    //        $("#validationMessage").children().children().html("").css("display", "block");
    //    }
    //});

    //$('#form1').find('#thirdAnswer').on('change keyup paste', function () {
    //    if ($('#firstAnswer').val().length == 0
    //        || $('#secondAnswer').val().length == 0
    //        || $('#thirdAnswer').val().length == 0
    //        ) {
    //        var text = "You have  " + getAttetmps() + " attempts remaining";
    //        $("#validationMessage").children().children().html(text).css("display", "block");
    //        return false;
    //    }
    //    else {
    //        $("#validationMessage").children().children().html("").css("display", "block");
    //    }
    //});

    //function getAttetmps() {
    //    var results = document.cookie.match('(^|;) ?' + 'secretQuestionsAttempts' + '=([^;]*)(;|$)');
    //    if (results)
    //        return (unescape(results[2]));
    //    else
    //        return null;
    //};

});
function backPage() {
    window.history.go(-1);
}