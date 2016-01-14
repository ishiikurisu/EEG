Implementa��o do SST
====================

<!-- Assumindo uma explica��o do teste, das medi��es realizadas e do porque realiz�-las. -->

Concep��o
---------

<!-- introdu��o -->
Para podermos implementar o SST, inicialmente fizemos um algoritmo do teste nos baseando nas descri��es do teste encontradas na literatura.<!-- Citar descri��es? --> A dizer, o teste consiste em um la�o de repeti��o: por um n�mero fixo de vezes, deve ser apresentado ao sujeito dois est�mulos visuais diferentes. Quando lhe aparecer estas imagens, o sujeito pode ou n�o ser estimulado com um sinal sonoro. Se o sinal sonoro aparecer, o sujeito n�o deve reagir. Caso contr�rio, ele deve responder como for especificado. Os est�mulos visuais devem ser apresentados em igual propor��o e at� que (1) o usu�rio responda (2) passe do tempo m�ximo permitido, enquanto que o sinal sonoro deve ser apresentado em uma parcela menor de vezes. No nosso caso, decidimos que

+ os est�mulos visuais seriam setas apontando para a direita e para a esquerda, para que o sujeito pudesse responder usando as teclas do teclado de um computador em no m�ximo 1000ms.
+ som seria apresentado em somente 25% das vezes<!-- Citar porque 25%? -->.

<!-- lidando com tempo -->
Como o intuito do teste � medir o tempo de demora para que uma pessoa iniba uma a��o j� iniciada, o sinal sonoro deve ser apresentado depois das setas. Definiremos o tempo entre a apresenta��o das setas e a do som como `SSD`. A fim de estimar o tempo lim�trofe entre o sujeito conseguir inibir ou n�o, o teste varia o `SSD` de acordo com o desempenho do sujeito: em caso de acerto, isto �, caso ele consiga inibir sua resposta, tornaremos o teste mais dif�cil aumentando o `SSD` em um valor fixo, que chamaremos de _passo_. Sen�o, subtra�mos um passo do `SSD`, fazendo com que o sinal sonoro seja apresentado mais perto do sinal visual. Com esta perspectiva, devemos definir:

+ SSD inicial
+ SSD m�nimo
+ SSD m�ximo
+ Passo

<!-- equa��o do ssrt, # de vezes -->
A partir da equa��o do `SSRT`, vamos abrir m�o da estat�stica para podermos definir, por amostragem, qual o `SSRT` do sujeito. Assumindo<!-- citar porque podemos assumir isso --> que o `SSRT` obedece a uma distribui��o normal em torno de um valor m�dio, que � o que queremos medir, temos que os par�metros que tangem o SSD e o passo devem ser ajustados para podermos amostrar bem a faixa em torno do valor desejado. Da literatura, observamos que o la�o principal do teste deve se repetir por cerca de 128 vezes, resultando em uma dura��o confort�vel para o sujeito e em um n�mero de respostas grande o suficiente para podermos aferir o SSD m�dio. Assim, optamos pelos seguintes par�metros de tempo para o teste:

+ SSD inicial: 250ms, no nosso caso.
+ SSD m�nimo: 0ms, ou seja, o sinal pode aparecer junto com as setas, se for necess�rio.
+ SSD m�ximo: 750ms, por ser 75% do tempo m�ximo.
+ Passo: 50ms

<!-- conclus�o para implementa��o -->
A partir deste entendimento, podemos escrever um pseudo-c�digo para explicar a nossa implementa��o do SST:

``` html
instru��es
para n = 1 at� 128
    mostrar intervalo
    mostrar seta
    se tem est�mulo sonoro
    se n�o
    fim se
fim para
```

<!-- conclus�o para o processamento -->
Ap�s cada aplica��o do teste, aplicamos um processamento individual nas medidas geradas usando a equa��o do SST: <!-- como aplicar  a equa��o-->. Com um n�mero suficientemente grande de sujeitos colaborando com o teste, podemos um estimar um `SSRT` coletivo.

Com isso, podemos finalmente come�ar a implemetar o SST. A ferramenta de escolha foi o software E-prime, por j� ser utilizado em testes psicol�gicos. As imagens foram geradas usando o software Processing; o som, usando o software Pure Data. Os dados foram processados usando programas pr�prios do laborat�rio escritos. Todo o c�digo-fonte pode ser acessado no site [](https://github.com/ishiikurisu/EEG/SST).

Implementa��o
-------------

aprendendo basic no e-prime. concep��o da arte. concep��o do som. processamento dos dados.

Adapta��o do teste para idosos
------------------------------

n�mero de aus�ncias como motiva��o para (1) tornar o teste mais devagar; e (2) explicar melhor o teste, baseando-se em tutoriais de videogames.

Refer�ncias
-----------

+ Criar lista de refer�ncias
+ Sim, por favor
