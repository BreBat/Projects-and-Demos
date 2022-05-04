<?php
    // return array of european countries starting with $letter
    // also remove them from $euro_countries_left
    function getCountries(& $euro_countries_left, $letter) {
        // if no countires are passed in, return empty array
        if (count($euro_countries_left) == 0) {
            return [];
        }

        // return this array
        $new_array = [];

        // while next one starts with $letter
        while ($euro_countries_left[0]['name'][0] == $letter) {
            array_push($new_array, array_shift($euro_countries_left));
        }

        // return array of associative arrays
        return $new_array;
    }

    // prints a group of countries
    function printSection (& $groupOfCountries, $class, $letter) {
        // print div tag
        echo '<div class="' . $class . '">';

        // print header
        echo '<h3>' . $letter . '</h3>';
        echo '<hr>';
        
        // print ul
        echo '<ul class="group_countries"';

        // print list items
        foreach ($groupOfCountries as $country) {
            echo '<li><a href="country-info.php?alpha=' . $country['alpha3Code'] . '">'
                 . $country['name'] . '</a></li>';
        }

        // end ul
        echo '</ul>';

        // end div tag
        echo '</div>';
    }

    // include header
    include('includes/header.php');

    // make request and get JSON back
    $euro_countries = file_get_contents('data/country-codes.json');

    // convert JSON to associative array
    $euro_countries = json_decode($euro_countries, true);
?>
<main id="list_of_countries">
<h2>European Countries</h2>

<?php
    // get letters A to Z
    $letters = range('A', 'Z');
    $groupOfCountries = [];
    $class = 'colOne';

    // iterate from A to Z
    foreach ($letters as $letter) {
        // get countries starting with next letter
        $groupOfCountries = getCountries($euro_countries, $letter);

        // if there are countries for this letter
        if (count($groupOfCountries) > 0) {
            //print section
            printSection($groupOfCountries, $class, $letter);

            // flip class
            $class = ($class == 'colOne') ? 'colTwo' : 'colOne';
        }
    }
    
    // print misc section
    printSection($euro_countries, $class, 'Misc');

    // clear floating
    echo '<div class="clear"></div>';

    // print end of main
    echo '</main>';

    // include footer
    include('includes/footer.php');
?>