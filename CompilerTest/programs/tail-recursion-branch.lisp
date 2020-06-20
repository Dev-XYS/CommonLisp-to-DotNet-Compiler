(defun f (x)
    (if (= x 0)
        0
        (if (= x 1)
            1
            (f (- x 2)))))
(writeln (f 11))
(writeln (f 12))
