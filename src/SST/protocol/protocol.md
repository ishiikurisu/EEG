Implementa��o do SST
====================

<!-- Assumindo uma explica��o do teste, das medi��es realizadas e do porque realiz�-las. -->

Nota��o
-------

Vamos denotar `<M>` como a m�dia aritm�tica da medida `M`.

Concep��o
---------

<!-- introdu��o -->
Para podermos implementar o SST, inicialmente fizemos um algoritmo do teste nos baseando em suas descri��es encontradas na literatura.<!-- Citar descri��es? --> A dizer, ele consiste em um la�o de repeti��o: por um n�mero fixo de vezes, deve ser apresentado ao sujeito dois est�mulos visuais diferentes. Quando lhe aparecer estas imagens, o sujeito pode ou n�o ser estimulado com um sinal sonoro. Se o sinal sonoro aparecer, o sujeito n�o deve reagir. Caso contr�rio, ele deve responder como lhe for especificado. Os est�mulos visuais devem ser apresentados em igual propor��o e at� que (1) o usu�rio responda; ou que (2) passe do tempo m�ximo permitido. J� o sinal sonoro deve ser apresentado em uma parcela menor de vezes. No nosso caso, decidimos que

+ os est�mulos visuais seriam setas apontando para a direita e para a esquerda, para que o sujeito pudesse responder usando as teclas do teclado de um computador em no m�ximo 1000ms.
+ som seria apresentado em somente 25% das vezes<!-- Citar porque 25%? -->.

<!-- lidando com tempo -->
Como o intuito do teste � medir o tempo de demora para que uma pessoa iniba uma a��o j� iniciada, o sinal sonoro deve ser apresentado depois das setas. Definiremos o tempo entre a apresenta��o das setas e a do som como `SSD`. A fim de estimar o tempo lim�trofe entre o sujeito conseguir inibir ou n�o, o teste varia o `SSD` de acordo com o desempenho do sujeito: em caso de acerto, isto �, caso ele consiga inibir sua resposta, tornaremos o teste mais dif�cil aumentando o `SSD` em um valor fixo, que chamaremos de _passo_. Sen�o, subtra�mos um passo do `SSD`, fazendo com que o sinal sonoro seja apresentado mais perto do sinal visual. Com esta perspectiva, devemos definir:

+ SSD inicial;
+ SSD m�nimo;
+ SSD m�ximo;
+ Passo.

<!-- equa��o do ssrt, # de vezes -->
A partir da equa��o do `SSRT`, vamos abrir m�o da estat�stica para podermos definir, por amostragem, qual o `SSRT` do sujeito. Assumindo<!-- citar porque podemos assumir isso --> que o `SSRT` obedece a uma distribui��o normal em torno de um valor m�dio, que � o que queremos medir, temos que os par�metros que tangem o SSD e o passo devem ser ajustados para podermos amostrar bem a faixa em torno do valor desejado. Da literatura, observamos que o la�o principal do teste deve se repetir por cerca de 128 vezes, resultando em uma dura��o confort�vel para o sujeito e em um n�mero de respostas grande o suficiente para podermos aferir o SSD m�dio. Assim, optamos pelos seguintes par�metros de tempo para o teste:

+ SSD inicial: 250ms, no nosso caso;
+ SSD m�nimo: 0ms, ou seja, o sinal pode aparecer junto com as setas, se for necess�rio;
+ SSD m�ximo: 750ms, por ser 75% do tempo m�ximo;
+ Passo: 50ms.

<!-- considera��es com o sujeito -->
Para que o sujeito pudesse realizar o teste com sucesso, adicionamos ao come�o do teste uma parte de instru��o e outra de treino. Originalmente, concebemos as instru��es como um breve texto explicativo, indicando o que o sujeito deveria fazer. A etapa de treinos, por sua vez, foi planejada para ser uma vers�o menor do teste, em que o `SSD` n�o se altera e em que h� menos repeti��es, cerca de 25% do teste original.

Al�m disso, adicionamos � cada itera��o do la�o principal uma resposta do programa ao sujeito: caso ele acerte, o programa indica uma resposta correta; caso ele erre, uma resposta negativa; caso ele se ausente, uma mensagem indicando que ele deve tentar responder mais r�pido.

<!-- conclus�o para implementa��o -->
A partir deste entendimento, podemos escrever um pseudo-c�digo para explicar a nossa implementa��o do SST:

    instru��es
    para n = 1 at� n�mero_de_vezes
        qual_seta <- aleat�rio
        deve_apertar? <- aleat�rio
        h�_resposta <- n�o

        mostrar intervalo
        mostrar qual_seta

        enquanto (n�o h�_resposta) ou (dentro do limite de tempo)
            se n�o deve_apertar?
                mostrar som quando for o tempo do SSD
            fim se
            h�_resposta <- checar por resposta
        fim enquanto

        se (houve resposta) e (n�o deve_apertar?)
            SSD <- SSD - passo
        sen�o
            SSD <- SSD + passo
        fim se

        mostrar resposta do programa
    fim para

<!-- conclus�o para o processamento -->
Ap�s cada aplica��o do teste, processamos as medidas geradas pelo indiv�duo usando a equa��o do SST `SSRT = RT - SSD`. As medidas de interesse foram o tempo de rea��o m�dio `<RT>`; o _delay_ m�dio `<SSD>` do est�mulo sonoro; a porcentagem de inibi��o `<%I>`; e a porcentagem de aus�ncias `<%A>`. Calculamos `<RT>` como sendo a soma de todos os `RT` dividida pelo n�mero de total de rea��es, ou seja, n�o levamos as aus�ncias em conta. `<SSD>` foi calculado como sendo o valor m�dio de todos os `SSD` apresentados ao longo do teste. `<%I>` � obtido como sendo o n�mero de vezes que o sujeito conseguiu inibir a a��o divido pelo n�mero total de apresenta��es do est�mulo para inibi��o. `<%A>` foi calculado como sendo o n�mero de vezes que o sujeito deixou de responder quando ele deveria dividido pelo n�mero total de apresenta��es sem o sinal de parada.

Com um n�mero suficientemente grande de sujeitos colaborando com o teste, podemos um estimar um `SSRT` coletivo, calculando a diferen�a entre a m�dia de todos os `<RT>` e a m�dia de todos os `<SSD>`.

<!-- finalmente... -->
Desta forma, podemos finalmente come�ar a implemetar o SST. A ferramenta de escolha foi o software E-prime, por j� ser utilizado em testes psicol�gicos. As imagens foram geradas usando o software Processing; o som, usando o software Pure Data. Os dados foram processados usando programas pr�prios do laborat�rio. Todo o c�digo-fonte pode ser acessado no endere�o web [](https://github.com/ishiikurisu/EEG/SST).

Implementa��o
-------------

<!-- aprendendo basic no e-prime. concep��o da arte. concep��o do som. processamento dos dados. -->
O pacote de programas _E-prime_ nos permite criar experimentos psicol�gicos com imagens; sons; e v�deos, com facilidades para medidas nos tempos de rea��o e exposi��o. O software _E-Studio_ possibilita ao usu�rio o uso destas ferramentas para a confec��o do teste, que, quando compilado, gera um _script_ em _Basic_, que ser� interpretado pelo _E-run_. Ap�s a execu��o do teste, o programa gera um arquivo, que pode ser traduzido para o formato `csv`, o qual usamos para poder analisar os dados por meio de programas escritos em C++ e em Java.

Para podermos organizar melhor o nosso c�digo-fonte, divimos o _loop_ principal do programa em dois procedimentos: o `PressProc`, que cont�m uma itera��o em que o sujeito deve responder a um est�mulo; e o `NotPressProc`, que consiste de uma repeti��o em que h� um sinal de parada indicando que o sujeito n�o deve apertar nenhum bot�o. Al�m disso, definimos uma vari�vel global para indicar qual a dura��o do `SSD` em uma dada itera��o. Essa vari�vel sempre come�a o programa com o valor de 250ms.

O `PressProc` � executado em tr�s passos:

+ mostrar uma tela vazia por 1000ms;
+ mostrar a seta enquanto o usu�rio n�o responde ou enquanto n�o se passar do tempo m�ximo;
+ mostrar o resultado da itera��o para o sujeito por 1000ms.

Definimos como par�metros do procedimento o caminho para a seta escolhida; e qual a tecla que corresponde � resposta correta. Caso o usu�rio responda corretamente, � lhe mostrado um s�mbolo de correto; em caso negativo, � lhe mostrado um `X` vermelho; caso ele se ausente, aparece uma mensagem na tela. Ap�s cada itera��o, o _script_ atualiza o arquivo de dados que cont�m as medidas realizadas pelo programa. Neste caso, optamos por verificar se houve ou n�o uma aus�ncia por parte o sujeito; e, caso ele tenha respondido, medimos o seu tempo de rea��o.

Embora similar, o `NotPressProc` possui um passo a mais, e sua l�gica muda um pouco na hora de mostrar o resultado para o sujeito:

+ mostrar uma tela vazia por 1000ms;
+ mostrar a seta enquanto o usu�rio n�o responde ou enquanto n�o se passar do tempo m�ximo;
+ mostrar o sinal sonoro ap�s um tempo correspondente ao `SSD` atual;
+ mostrar o resultado da itera��o ao sujeito por 1000ms.

O �nico par�metro deste procedimento � o caminho para a imagem da seta escolhida, j� que � esperado que o sujeito iniba a a��o independe da dire��o da seta. Se ele responder, o teste devolve uma corre��o negativa; caso contr�rio, o ele mostra o s�mbolo de correto designado. O procedimento, por fim, atualiza o valor do `SSD` para a pr�xima itera��o em que for necess�rio inibir; adiciona ao arquivo com os resultados se houve inibi��o ou n�o, e qual a dura��o do `SSD` naquela repeti��o.

Estes procedimentos foram colocados em outros dois procedimentos maiores: a pr�tica e o experimento em si. Ambos s�o listas que cont�m 32 e 128 itens, respectivamente, populadadas com 3/4 de seus itens com `PressProc` e 1/4 de `NotPressProc`. Metade deles possuem setas para a esquerda; e a outra metade, para a direita. O _E-run_ ent�o gera, para cada execu��o do teste, uma permuta��o destas listas para que todos as aplica��o sigam uma ordem pr�pria, e com as propor��es que desejamos.

Finalmente, adicionamos as intru��es ao come�o do teste e uma mensagem de agradecimento ao fim dele.

Adapta��o do teste para idosos
------------------------------

n�mero de aus�ncias como motiva��o para (1) tornar o teste mais devagar; e (2) explicar melhor o teste, baseando-se em tutoriais de videogames.

Refer�ncias
-----------

+ Criar lista de refer�ncias
+ Sim, por favor
