: example-input ( reverse order, split into pairs )
    2121212118 2121212124
    824824821 824824827
    565653 565659
    38593856 38593862
    446443 446449
    1698522 1698528
    222220 222224
    1188511880 1188511890
    998 1012
    95 115
    11 22
;

: digit-count ( n -- n ) 1 swap begin 10 / dup 0 > if swap 1 + swap then dup 0= until drop ;

: ?odd ( n -- bool ) 2 mod 1 = ;

: ?valid-id ( n -- bool )
    dup digit-count

    dup ?odd if
        2drop
        true
        exit
    then

    1
    swap

    2 /
    0
    do
        10 *
    loop

    /mod = invert
;

: invalid-ids-in-range ( from to -- valid1 valid2 ... n )
    0 rot rot
    1 +
    swap
    do
        i ?valid-id invert
        if
            i
            swap
            1 +
        then
    loop
;

: solve ( a_from a_to b_from b_to ... -- res )
    0

    depth 2 /
    0
    do
        rot rot invalid-ids-in-range
        0
        ?do
            +
        loop
    loop
;
