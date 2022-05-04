<?php include('includes/header.php'); ?>

<style>
    div.theImage{
        width: 300px;
        background-color: white;
        box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        margin-bottom: 25px;
        margin-left: auto;
        margin-right: auto;
        padding-top: 2%;
    }
    
    div.container {
        text-align: center;
        padding: 10px 20px;
    }

    img.pepe{
        width: auto;
    }

    h1.test{
        text-align: center;
    }

    span.everything{
        text-align: center;
    }
</style>

<span class="everything"> 
    <h1 class="test">404 Not Found: Page Not Found</h1>

    <h2>Invalid URL Found: Unable to find matching Country Code.</h2>

    <br>

    <div class="theImage">
        <img src="images/FeelsBadMan.png" alt="FeelsBadMan..." class="pepe">

        <div class="container">
            <p><a href="list.php">Country List</a></p>
        </div>
    </div>
    

</span>


<?php include('includes/footer.php'); ?>
