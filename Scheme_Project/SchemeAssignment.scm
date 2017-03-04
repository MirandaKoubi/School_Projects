;Name:          Miranda Koubi
;CLID:          mrk3865
;Class:         CMPS 450	Fall 2014
;Due Date:      October 30, 2014 at 12:30 pm
;Assignment:    
;A meta-circular interpreter for Scheme is an interpreter for Scheme written in Scheme.
;The following code, adapted from R. Kent Dybvig's book "The Scheme Programming Language", 
;is a partial meta-circular interpreter for Scheme. 
;This interpreter clearly shows how compact the language is. In Scheme, 
;the derived expressions of the language are traditionally expanded into their 
;equivalent core expressions prior to interpretation. The interpreter, listed below, 
;utilizes the function "expand" to translate the derived expressions in Scheme into the
;primitive expressions as defined in section 7.3 of the 4th revised report on the language.

;However, "expand" is not part of the language definition and hence, not implemented in 
;all scheme interpreters. Your task is to implement "expand". "expand" should take any 
;legal scheme expression and return that expression, unaltered, if it consists of only 
;the primitive core expressions or return the equivalent primitive core expressions if 
;the input expression contains any of the derived expressions: cond, case, and, or, let, 
;let*, letrec. For this assignment, ignore the derivative for cond which uses the recipient.

;You will also need to provide definitions for the functions lookup, assign, 
;and new-env to complete the implementation of the environment within the interpreter. 
;The top level support funtions are provided with a sample top level environment.
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;WRITTEN AND TESTED IN REPLT.IT;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

(define interpret #f)

(define top-level-env `( (2 ,2) (3 ,3) (< ,<) (> ,>)))

(define top-level-value 
              (lambda (id) 
                  (let (( pair (assq id top-level-env)))
                        (if pair
                            (cadr pair)
                            (id)))))

(define set-top-level-value! 
              (lambda (id val) 
                  (let (( pair (assq id top-level-env)))
                        (if (null? pair)
                            (id)
                            (set! top-level-env (cons (list id val) top-level-env))))))


(define expand 
    (lambda (exp)
        (cond
            ((symbol? exp) exp)
            ((number? exp) exp)
            ((pair? exp)
                (case (car exp)
                    ((let)
                        (cons (list 'lambda (map expand (map car (cadr exp)))
                        (expand (caddr exp)))
                        (map expand (map cadr (cadr exp))))
                    )
				
					((let*)
                        (if (pair? (cadr exp))
                            (expand `(let ((,@(expand (caadr exp))))
                            ,(expand `(let* (,@(expand (cdadr exp)))
                            ,(map expand (caddr exp))))))
                            
                            `((lambda () ,@(cddr exp)))
                        )
                    )

					((and)
                        (if (null? (cdr exp))
                            '#t
                            (if (null? (cddr exp))
                                (cadr exp)
								
                                (expand `(let ((x ,(expand (cadr exp)))
                                (thunk ((lambda () 
                                ,(expand `(and ,(map expand (caddr exp))))))))
                                (if x thunk x)))
                            )
                        )
                    )
                    
                    ((or)
                        (if (null? (cdr exp))
                            '#f
                            (if (null? (cddr exp))
                                (cadr exp)
								
                                (expand `(let ((x ,(expand (cadr exp)))
                                (thunk ((lambda () 
                                ,(expand `(and ,(map expand (caddr exp))))))))
                                (if x x thunk)))
                            )
                        )
                    )
					
					((cond)
                        (if (null? (cdr exp))
                            'unspecified
                            (if (equal? (caadr exp) 'else)
                                `(begin ,@(expand (cdadr exp)))
                                (if (null? (cdadr exp))
                                    (expand `(or ,@(expand (caadr exp)) 
                                    ,(expand `(cond ,@(map expand (cddr exp))))))
                                    
                                    `(if ,(expand (caadr exp)) 
                                    (begin ,@(expand (cdadr exp)))
                                    ,(expand `(cond ,@(map expand (cddr exp)))))
                                )
                            )
                        )
                    )
				
				   ((case)
                        (if (memq 'else (car (reverse exp)))
                            `(let ((key ,(cadr exp)) 
                                (thunk1 (lambda () ,@(expand(cdaddr exp))))
                                (elsethunk (lambda () 
                                ,@(expand (cdar (reverse exp))))))
                                (cond (( memv key (quote)(,@(expand (caaddr exp)))) 
								(thunk1)) (else (elsethunk))))
                                
                            `(let ((key ,(cadr exp))
                                (thunk1 ((lambda () ,@(expand (cdaddr exp))))))
                                (cond ((memv key (quote)
								(,@(expand (caaddr exp)))) thunk1)))
                        )
                    )
					
                ;If the first element of the exp is "letrec", the the case that should be
				;here would happen. The letrec read in would be turned into a let 
				;containing the tuples of each variable followed by an undefined character.
				;In that let expression would be another let containing tuples of unique
				;temp variables paired with the inits of the variables. Then a set! would
				;be declared containing all of the unique variables and temps. After that
				;would be the body of the first let. Both of the lets and every element 
				;of the expression would be expanded to make fure that no derived 
				;expressions when the expression is interpreted.
				
                (else (map expand exp))
                
                )
            )
            
            (else exp)
        )
    )
)

;The following letrec contains functions that are used by exec to complete the 
;interpretation of given expressions.

(letrec
   ((new-env
    (lambda (idl vals env)
            (if (null? idl)
                env
                (cons(list (car idl) (car vals))
			        (new-env (cdr idl) (cdr vals) env))
			)
	))

   (lookup
    (lambda (id env)
        (let (( pair (assq id env)))
    		(if pair
				(cadr pair)
				(top-level-value id)
			)
		)
	))   
 
   (assign
    (lambda (id val env)
    	(let (( pair (assq id env)))
    		(if pair
				(set-cdr! pair val)
				(set-top-level-value! id val)
			)
		)
	))

;Exec is a function that was given and does not need to be documented.
   (exec
    (lambda (exp env)
      (cond
           ((symbol? exp) (lookup exp env))
           ((number? exp) (lookup exp env))
           ((pair? exp) 
             (case  (car exp)
                   ((quote) (cadr exp))
                   ((lambda)
                          (lambda vals
                                 (exec (cons 'begin (cddr exp))
                                           (new-env (cadr exp) vals env))))
                   ((if) 
                          (if (exec (cadr exp) env)
                              (exec (caddr exp) env)
                              (exec (cadddr exp) env)))
                   ((set!)
                          (assign (cadr exp)
                                      (exec (caddr exp) env)
                                       env))
                   ((begin)
                          (let loop ((exps (cdr exp)))
                              (if (null? (cdr exps))
                                  (exec (car exps) env)
                                  (begin
                                       (exec (car exps) env)
                                       (loop (cdr exps))))))
                   (else
                          (apply (exec (car exp) env)
                                    (map (lambda (x)  (exec x env))
                                             (cdr exp))))))
           (else exp)))))

(set! interpret
      (lambda (exp)
            (exec (expand exp) '()))))
            
(interpret `(let ((x 2) (y 3)) (and (> y x) 'greater)))