: example-input -82 14 -99 -1 -55 60 -5 48 -30 -68 ; ( reverse order, L=-, R=+ )
: step ( a b acc -- c acc' )
    rot rot + 100 mod
    dup 0= if swap 1 + else swap then ;
: solve ( a b c ... -- res )
    50 0 depth 2 - 0 do step loop . drop ;
