<?php

class Position {
    public $x;
    public $y;

    public function __construct($x, $y) {
        $this->x = $x;
        $this->y = $y;
    }

}

class Operand {
    
    public $value;
    public $position;

    public function __construct($value, $position) {
        $this->value = $value;
        $this->position = $position;
    }

}

class Operator {
    public $type;
    public $position;

    public function __construct($type, $position) {
        $this->type = $type;
        $this->position = $position;
    }

}

class Equal {
    public $position;

    public function __construct($position) {
        $this->position = $position;
    }

}

class Stage {

    public $moves;
    public $operands = [];
    public $operators = [];
    public $equals = [];

}

function get(): string {
    $text = fgets(STDIN);
    $text = str_replace(array("\r", "\n"), '', $text);
    return $text;
}

$stage = new Stage;

echo "stage index = ";
$stage_index = get();

$file_name = "stage$stage_index.json";

echo "moves = ";
$moves = get();
$stage->moves = $moves;

for ($y = 0; $y < 5; ++$y) {
    for ($x = 0; $x < 7; ++$x) {
        echo "($x, $y) = ";
        $position = new Position($x, $y);
        $val = get();
        if (preg_match("/[0-9]/u", $val)) {
            $stage->operands[] = new Operand($val, $position);
        } else if (preg_match("/[\\+\\-\\*\\/]/u", $val)) {
            $map = ["+"=>0, "-"=>1, "*"=>2, "/"=>3];
            $stage->operators[] = new Operator($map[$val], $position);
        } else if ($val == "=") {
            $stage->equals[] = new Equal($position);
        }
    }
}

file_put_contents($file_name, json_encode($stage));