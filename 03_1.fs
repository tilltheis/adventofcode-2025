15 constant example-bank-length

4 constant example-bank-count

create example-banks
    9 , 8 , 7 , 6 , 5 , 4 , 3 , 2 , 1 , 1 , 1 , 1 , 1 , 1 , 1 ,
    8 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 9 ,
    2 , 3 , 4 , 2 , 3 , 4 , 2 , 3 , 4 , 2 , 3 , 4 , 2 , 7 , 8 ,
    8 , 1 , 8 , 1 , 8 , 1 , 9 , 1 , 1 , 1 , 1 , 2 , 1 , 1 , 1 ,

: example-input ( -- addr w h )
    example-banks example-bank-length example-bank-count ;

: max-joltage ( addr n -- max-joltage )
    cells over +
    dup 1 cells - swap rot ( remember last element )
    0 0 ( tens ones )
    2swap
    do
        over i @ <
        3 pick i > ( not last element )
        and
        if
            2drop
            i @
            i 1 cells + @
        else
            dup i @ <
            if
                drop i @
            then
        then

        1 cells
    +loop
    rot drop
    swap 10 * +
;

: rot4 ( a b c d -- b c d a )
   >r rot r> swap ;


: solve ( addr w h -- res )
    0 rot4 rot4 rot4
    0
    do
        over over i cells * +
        over
        max-joltage
        rot4 + rot rot
    loop
    2drop
;
