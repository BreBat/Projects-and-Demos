<?php
    // make sure alpha3code is from Europe
    function codeIsEuropean($code) {
        // grab european coutnry codes
        $euro_codes = file_get_contents('data/country-codes.json');
        $euro_codes = json_decode($euro_codes, true);

        // iterate through european alpha 3 codes
        foreach ($euro_codes as $euro_code) {
            // $code is a valid european alpha3Code
            // capitalize $code since anything passed will be lower case
            // and all stored codes are upper case
            if ($euro_code['alpha3Code'] === strtoupper($code)) {
                return true;
            }
        }

        // didn't match any of them i.e., not valid
        return false;
    }

    // input: alpha3Code
    // output: country name
    function getCountryName($code) {
        // get european country codes
        $euro_codes = file_get_contents('data/country-codes.json');

        // turn JSON string into an associative array
        $euro_codes = json_decode($euro_codes, true);

        // iterate thought european country codes
        foreach ($euro_codes as $euro_code) {
            // see if we found match
            if ($euro_code['alpha3Code'] === $code) {
                return $euro_code['name'];
            }
        }

        // $code isn't an european alpha3Code
        echo 'Ut-Oh. Wasn\'t supposed to get this far. Exiting program...';
        exit();
    }

    // alaph is the only parameter
    if (isset($_GET['alpha']) && $_GET['alpha'] != '' && count($_GET) === 1) {
        // get alpha3Code
        $code = $_GET['alpha'];

        // if code is valid and european
        if (codeIsEuropean($code)) {
            // url for web api request; $_GET['alpha'] is country code
            $url = 'https://restcountries.eu/rest/v2/alpha/' . $code;
            $url .= '?fields=name;capital;subregion;population;latlng;borders;flag';

            // do request and get back JSON string
            $country_info = file_get_contents($url);

            // turn JSON string into associative array
            $country_info = json_decode($country_info, true);
        }

        // not european
        else {
            header('Location: 404.php');
            exit();
        }
    }

    else {
        header('Location: 404.php');
        exit();
    }

// include header.php
include('includes/header.php');

?>

<main>
<a href="<?php echo $country_info['flag']; ?>">
    <img id="flag" src="<?php echo $country_info['flag']; ?>"
        alt="Image of Flag of <?php echo $country_info['name']; ?>"
        title="Flag of <?php echo $country_info['name']; ?>">
</a>
<h2 id="country_name">
    &nbsp;&nbsp;<?php echo $country_info['name']; ?>
</h2>
<div class="clear"></div>
<table id="country_data">
    <tr>
        <th>Capital</th>
        <td><?php echo $country_info['capital']; ?></td>
    </tr>
    <tr>
        <th>Subregion</th>
        <td><?php echo $country_info['subregion']; ?></td>
    </tr>
    <tr>
        <th>Population</th>
        <td><?php echo number_format($country_info['population']); ?></td>
    </tr>
    <tr>
        <th>Latitude</th>
        <td><?php echo $country_info['latlng'][0]; ?></td>
    </tr>
    <tr>
        <th>Longitude</th>
        <td><?php echo $country_info['latlng'][1]; ?></td>
    </tr>
</table>
<h3>Bordering Countries</h3>
<ul id="bordering_countries">
    <?php
        // count number of bordering countries
        $num_bordering_euro = 0;

        // for each bordering country
        foreach ($country_info['borders'] as $country_code) {
            // test to see if code is european
            if (codeIsEuropean($country_code)) {
                // get name of country
                $country_name = getCountryName($country_code);

                // echo list element in unordered list
                echo "<li><a href=\"country-info.php?alpha=$country_code\">$country_name</a></li>";

                // increment number of bordering countries
                $num_bordering_euro += 1;
            }
        }
    ?>
</ul>
<?php
    // no bordering european countries
    if ($num_bordering_euro == 0) {
        echo '<p id="no_bordering_euro_countries">No bordering European countries.';
    }
?>
</main>
<?php
    include('includes/footer.php');
?>