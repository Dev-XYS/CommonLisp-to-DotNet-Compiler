(defun f (x) (if (> x 0) (f (- x 1)) NIL))
(f 10000000)
