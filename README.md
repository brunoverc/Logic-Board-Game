# Jogo de Tabuleiro Lógico

## Visão Geral
O Jogo de Tabuleiro Lógico é um jogo educativo desenvolvido em Unity que integra conceitos de lógica proposicional e lógica de predicados. O objetivo do jogo é ajudar os jogadores a aprender e praticar esses conceitos de maneira divertida e interativa.

<img width="901" alt="Screenshot 2024-06-26 at 17 22 07" src="https://github.com/brunoverc/Logic-Board-Game/assets/69854207/2c0b5b43-d865-4abc-8e9c-27593e8ae6ac">

## Como Jogar
1. **Início do Jogo**: Cada jogador começa na casa 1 do tabuleiro.
2. **Movimento**: Os jogadores lançam um dado e avançam o número correspondente de casas.
3. **Casas Especiais**: Algumas casas contêm problemas de lógica proposicional (fase 1) ou lógica de predicados (fase 2). 
   - **Problema de Lógica**: Se o jogador resolver corretamente o problema, mantém as casas avançadas. Caso contrário, retorna X casas conforme a penalidade da casa.
   - **Bônus e Penalidades**: Algumas casas oferecem bônus (por exemplo, avance X casas) ou penalidades (por exemplo, volte X casas).
4. **Fim do Jogo**: O primeiro jogador a chegar à casa 51 vence o jogo.

## Conceitos de Lógica
- **Lógica Proposicional**: Envolve operações lógicas básicas como AND, OR, NOT e IMPLICATION.
- **Lógica de Predicados**: Estende a lógica proposicional lidando com predicados e quantificadores (por exemplo, para todos, existe).

## Estrutura do Projeto
- **Scripts**: Contém todos os scripts C# que implementam a lógica do jogo.
- **Scenes**: Contém as cenas do Unity, incluindo o tabuleiro do jogo e interfaces de usuário.
- **Prefabs**: Contém prefabs reutilizáveis, como peças de jogo, dados e elementos de UI.
- **Assets**: Contém assets como imagens, sons e fontes.

## Requisitos do Sistema
- **Unity**: Versão 2021.3 ou superior
- **Plataforma**: Windows, MacOS, ou Linux

## Instalação
1. Clone o repositório:
   ```bash
   git clone https://github.com/usuario/jogo-tabuleiro-logico.git
2. Abra o projeto no Unity:
- Inicie o Unity Hub.
- Clique em "Add" e selecione a pasta do projeto clonada.
- Abra o projeto.

## Contribuição

1. Fork o projeto.
2. Crie uma branch para sua feature (git checkout -b feature/sua-feature).
3. Commit suas mudanças (git commit -m 'Adicionar feature X').
4. Push para a branch (git push origin feature/sua-feature).
5. Abra um Pull Request.
