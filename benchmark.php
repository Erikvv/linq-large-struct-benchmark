<?php

ini_set("memory_limit", "12G");

class SmallClass
{
    public $i;

    public function __construct()
    {
        $this->i = random_int(0,  PHP_INT_MAX);
    }
}

// 136 bytes
class LargeClass
{
    public $a = 1;
    public $b = 2;
    public $c = 3;
    public $d = 4;
    public $e = 5;
    public $f = 6;
    public $g = 7;
    public $h = 8;
    public $j = 9;
    public $k = 10;
    public $l = 11;
    public $m = 12;
    public $n = 13;
    public $o = 14;
    public $p = 15;
    public $q = 16;

    public $i;

    public function __construct()
    {
        $this->i = random_int(0,  PHP_INT_MAX);
    }
}

echo "initializing array\n";

$items = array_map(
    function ($i) {
        return new SmallClass();
    },
    range(1, 10000000)
);

$threshold = PHP_INT_MAX / 2;
//$threshold = PHP_INT_MAX - (PHP_INT_MAX / 10000000 * 5);

echo "starting benchmark\n";
$before = microtime(true);

$filtered = array_filter($items, function ($sc) use ($threshold) {
    return $sc->i < $threshold;
});

$after = microtime(true);

echo count($filtered) . " items\n";
echo ($after-$before) . " seconds\n";
