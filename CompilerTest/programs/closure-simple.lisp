(defun make-closure (x)
    (lambda (y) (+ x y)))
(let ((c (make-closure 5))) (writeln (c 3)))
