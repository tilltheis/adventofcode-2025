require test/ttester.fs

\ false = empty cell, true = occupied cell
create example-input
    10 , 10 , \ width height
    false , false , true  , true  , false , true  , true  , true  , true  , false ,
    true  , true  , true  , false , true  , false , true  , false , true  , true  ,
    true  , true  , true  , true  , true  , false , true  , false , true  , true  ,
    true  , false , true  , true  , true  , true  , false , false , true  , false ,
    true  , true  , false , true  , true  , true  , true  , false , true  , true  ,
    false , true  , true  , true  , true  , true  , true  , true  , false , true  ,
    false , true  , false , true  , false , true  , false , true  , true  , true  ,
    true  , false , true  , true  , true  , false , true  , true  , true  , true  ,
    false , true  , true  , true  , true  , true  , true  , true  , true  , false ,
    true  , false , true  , false , true  , true  , true  , false , true  , false ,

: 3dup ( a b c -- a b c a b c ) 2 pick 2 pick 2 pick ;

: 3drop ( a b c -- ) drop drop drop ;

: rot4 ( a b c d -- b c d a ) >r rot r> swap ;
: -rot4 ( a b c d -- d a b c ) swap >r -rot r> ;

: width ( map-addr -- w ) @ ;
: height ( map-addr -- h ) cell+ @ ;
: size ( map-addr -- size ) dup width swap height * ;
: offset ( map-addr x y -- offset ) rot width * + ;
: ?occupied-offset-unsafe ( map-addr offset -- cell ) 1 cells * 2 cells + + @ ;

: ?valid-coord ( map-addr x y -- flag )
    dup 0 < if 3drop false exit then
    over 0 < if 3drop false exit then
    2 pick height >= if 2drop false exit then
    swap width >= if false else true then
;

: ?occupied-coord ( map-addr x y -- flag )
    3dup ?valid-coord if
        2 pick >r
        offset
        r> swap ?occupied-offset-unsafe
    else
        3drop false
    then
;

: count-adjacent-occupied-neighbors ( map-addr x y -- count )
    0 -rot4
    2 -1 do
        2 -1 do
            i 0 <> j 0 <> or if
                3dup 
                i + swap j + swap
                ?occupied-coord if
                    rot4 1+ -rot4
                then
            then
        loop
    loop
    2drop drop
;

: solve ( map-addr -- res )
    0 swap
    dup height 0 do
        dup width 0 do
            dup j i ?occupied-coord if
                dup j i count-adjacent-occupied-neighbors
                4 < if
                    swap 1+ swap
                then
            then
        loop
    loop
    drop
;


testing width
T{ here 3 , width -> 3 }T

testing height
T{ here 3 , 7 , height -> 7 }T

testing size
T{ here 3 , 7 , size -> 21 }T

testing ?occupied-offset-unsafe
T{ here 2 , 2 , true , 0 ?occupied-offset-unsafe -> true }T
T{ here 2 , 2 , true , false , 1 ?occupied-offset-unsafe -> false }T
T{ here 2 , 2 , true , false , true , 2 ?occupied-offset-unsafe -> true }T
T{ here 2 , 2 , true , false , true , false , 3 ?occupied-offset-unsafe -> false }T

testing ?valid-coord
T{ here 2 , 1 , -1 -1 ?valid-coord -> false }T
T{ here 2 , 1 , 0 -1 ?valid-coord -> false }T
T{ here 2 , 1 , -1 0 ?valid-coord -> false }T
T{ here 2 , 1 , 0 0 ?valid-coord -> true }T
T{ here 2 , 1 , 1 0 ?valid-coord -> true }T
T{ here 2 , 1 , 2 0 ?valid-coord -> false }T
T{ here 2 , 1 , 0 1 ?valid-coord -> false }T

testing ?occupied-coord
T{ here 2 , 2 , true , true , true , true , -1 -1 ?occupied-coord -> false }T
T{ here 2 , 2 , true , true , true , true , 0 -1 ?occupied-coord -> false }T
T{ here 2 , 2 , true , true , true , true , 1 -1 ?occupied-coord -> false }T
T{ here 2 , 2 , true , true , true , true , 2 -1 ?occupied-coord -> false }T
T{ here 2 , 2 , true , true , true , true , -1 0 ?occupied-coord -> false }T
T{ here 2 , 2 , true , true , true , true , 0 0 ?occupied-coord -> true }T
T{ here 2 , 2 , true , true , true , true , 1 0 ?occupied-coord -> true }T
T{ here 2 , 2 , true , true , true , true , 2 0 ?occupied-coord -> false }T
T{ here 2 , 2 , true , true , true , true , -1 1 ?occupied-coord -> false }T
T{ here 2 , 2 , true , true , true , true , 0 1 ?occupied-coord -> true }T
T{ here 2 , 2 , true , true , true , true , 1 1 ?occupied-coord -> true }T
T{ here 2 , 2 , true , true , true , true , 2 1 ?occupied-coord -> false }T
T{ here 2 , 2 , true , true , true , true , -1 2 ?occupied-coord -> false }T
T{ here 2 , 2 , true , true , true , true , 0 2 ?occupied-coord -> false }T
T{ here 2 , 2 , true , true , true , true , 1 2 ?occupied-coord -> false }T
T{ here 2 , 2 , true , true , true , true , 2 2 ?occupied-coord -> false }T

T{ here 2 , 2 , true , true , true , false , 1 1 ?occupied-coord -> false }T

testing rot4
T{ 1 2 3 4 rot4 -> 2 3 4 1 }T

testing -rot4
T{ 1 2 3 4 -rot4 -> 4 1 2 3 }T

testing count-adjacent-occupied-neighbors
T{ here 3 , 3 ,
        false , false , false ,
        false , false , false ,
        false , false , false ,
        1 1 count-adjacent-occupied-neighbors -> 0 }T
T{ here 3 , 3 ,
        true  , false , false ,
        false , false , false ,
        false , false , false ,
        1 1 count-adjacent-occupied-neighbors -> 1 }T
T{ here 3 , 3 ,
        true , true , true ,
        true , true , true ,
        true , true , true ,
        1 1 count-adjacent-occupied-neighbors -> 8 }T
T{ here 3 , 3 ,
        true , true , true ,
        true , true , true ,
        true , true , true ,
        0 0 count-adjacent-occupied-neighbors -> 3 }T

testing solve
T{ example-input solve -> 13 }T