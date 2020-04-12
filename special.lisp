(special *global*)
(special *f*)
(setq *global* 5 *f* (lambda () (writeln *global*)))
(*f*)
(let ((*global* 7))
	(*f*)
	(setq *global* (+ 1 *global*))
	(*f*))
(*f*)
;;output should be:
;;5
;;7
;;8
;;5