require test/ttester.fs

15 constant example-bank-length

4 constant example-bank-count

create example-banks
    9 , 8 , 7 , 6 , 5 , 4 , 3 , 2 , 1 , 1 , 1 , 1 , 1 , 1 , 1 ,
    8 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 9 ,
    2 , 3 , 4 , 2 , 3 , 4 , 2 , 3 , 4 , 2 , 3 , 4 , 2 , 7 , 8 ,
    8 , 1 , 8 , 1 , 8 , 1 , 9 , 1 , 1 , 1 , 1 , 2 , 1 , 1 , 1 ,

: example-input ( -- addr w h )
    example-banks example-bank-length example-bank-count ;

: arr-max ( addr n -- max-addr max )
    >r >r r@ 0 r> r>    \ max-addr max addr n
    cells over +        \ max-addr max addr excl-end-addr
    swap                \ max-addr max excl-end-addr addr
    do                  \ max-addr max
        dup i @ <
        if
            2drop i i @ \ max-addr' max'
        then
        1 cells
    +loop
;

: max-joltage ( addr n battery-count -- max-joltage )
\ Greedily finds the largest subsequence of length battery-count.
\ In each step, it searches for the maximum value in the valid range (allowing enough space for remaining digits),
\ appends it to the result, and advances the start position to just after the found element.
    >r >r >r 0 r> r>            \ sum addr n
    cells over +                \ sum addr excl-end-addr
    cell+ dup r> cells -        \ sum addr excl-end-addr' excl-step-end-addr
    do                          \ sum addr
        dup i swap - 1 cells /  \ sum addr step-n
        arr-max                 \ sum addr' max
        rot 10 * + swap cell+   \ sum' addr'
        1 cells
    +loop
    drop
;

: rot4 ( a b c d -- b c d a )
   >r rot r> swap ;


: solve ( addr w h -- res )
    0 rot4 rot4 rot4
    0
    do
        over over i cells * +
        over
        12 max-joltage
        rot4 + rot rot
    loop
    2drop
;


testing arr-max

T{ here 1 , 2 , 3 , 3 , 2 , 1 , 6 arr-max nip -> 3 }T
T{ here here 1 , 2 , 3 , 3 , 2 , 1 , 6 arr-max drop swap - -> 2 cells }T


testing max-joltage

T{ here 9 , 8 , 7 , 6 , 5 , 4 , 3 , 2 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 15 2 max-joltage -> 98 }T
T{ here 8 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 9 , 15 2 max-joltage -> 89 }T
T{ here 2 , 3 , 4 , 2 , 3 , 4 , 2 , 3 , 4 , 2 , 3 , 4 , 2 , 7 , 8 , 15 2 max-joltage -> 78 }T
T{ here 8 , 1 , 8 , 1 , 8 , 1 , 9 , 1 , 1 , 1 , 1 , 2 , 1 , 1 , 1 , 15 2 max-joltage -> 92 }T

T{ here 9 , 8 , 7 , 6 , 5 , 4 , 3 , 2 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 15 12 max-joltage -> 987654321111 }T
T{ here 8 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 9 , 15 12 max-joltage -> 811111111119 }T
T{ here 2 , 3 , 4 , 2 , 3 , 4 , 2 , 3 , 4 , 2 , 3 , 4 , 2 , 7 , 8 , 15 12 max-joltage -> 434234234278 }T
T{ here 8 , 1 , 8 , 1 , 8 , 1 , 9 , 1 , 1 , 1 , 1 , 2 , 1 , 1 , 1 , 15 12 max-joltage -> 888911112111 }T


testing solve

T{ example-input solve -> 3121910778619 }T