if (!require("BiocManager", quietly = TRUE))
  install.packages("BiocManager")
BiocManager::install(version = "3.16")

if (!requireNamespace("igraph", quietly = TRUE))
  install.packages("igraph")

if (!requireNamespace("qgraph", quietly = TRUE))
  install.packages("qgraph")

if (!require("BiocManager", quietly = TRUE))
  install.packages("BiocManager")

if (!requireNamespace("gRbase", quietly = TRUE))
  install.packages("gRbase")


#graficzne przedstawienie macierzy kowariancji w pakiecie qgraph
#najpierw wezmy fajne dane
if (!requireNamespace("SemiPar", quietly = TRUE))
  install.packages("SemiPar")
library("qgraph")

data(milan.mort,package="SemiPar")
cor(milan.mort)
qgraph(cor(milan.mort), layout="groups")


####### Pakiet igraph #######
library("igraph")

#mozemy tworzyc grafy poprzez zadawanie krawedzi

g <- graph( edges=c(1,2, 2,3, 3,1, 4,1), directed=T)  #graf skierowany
plot(g)

g <- graph( edges=c(1,2, 2,3, 3,1, 4,1), directed=F) #graf nieskierowany
plot(g)

#lub poprzez zadanie macierzy incydencji
p<- 6;
A <- matrix( sample(c(0,1), p^2, replace=T, prob=c(0.6, 0.4)),p,p)
A

G <- graph_from_adjacency_matrix(A)
plot(G) #domyslnie tworzy graf skierowany 

diag(A) <- 0 #jesli nie chcemy malych petli 
G <- graph_from_adjacency_matrix(A)
plot(G)

G <- graph_from_adjacency_matrix(A, mode="min") #jesli A nie jest symetryczna to krawedz stawiamy tylko gdy sa w obie strony. 
plot(G)

G <- graph_from_adjacency_matrix(A, mode="max") #jesli A nie jest symetryczna to krawedz stawiamy gdy jest w obie strony
plot(G)

#mozemy latwo tworzyc drzewa ze stala liczba dzieci
drzewo <- make_tree(39, children=3, mode="undirected") #n-liczba wiercholkow
plot(drzewo, vertex.size = 10, vertex.label=NA)

#grafy Erdosa-Renyia
ERgraph <- sample_gnm(n=100, m=200) #n - liczba wierzcholkow, m=liczba krawedzi
plot(ERgraph, vertex.size=6, vertex.label=NA)

ERgraph <- sample_gnp(n=100, p=0.1) #n - liczba wierzcholkow, p=p-stwo kazdej krawedzi
plot(ERgraph, vertex.size=6, vertex.label=NA)



####### Pakiet gRbase #######
library(gRbase)
#mozemy tworzyc grafy nieskierowane przez zadanie klik maksymalnych
ug0 <- ug(c(1,2,3),c(1,3,4),c(5))
plot(ug0)
#dodajemy i usuwamy krawedzie
ug0 <- addEdge("1","5", ug0)
ug0 <- removeEdge("3","4", ug0)
try(dev.off(dev.list()["RStudioGD"]), silent=TRUE)
plot(ug0)
#dostep do wierzcholkow i krawedzi
nodes(ug0)
edges(ug0)
#kliki
is.complete(ug0)
is.complete(ug0, c("1","2","3"))
library(RBGL)
RBGL::maxClique(ug0)
#podgrafy indukowane
ug0_ <- subGraph(c("1","2","3","4"), ug0)
try(dev.off(dev.list()["RStudioGD"]), silent=TRUE)
plot(ug0_)
#simplicialnosc wierzcholka
is.simplicial("1", ug0)
is.simplicial("2", ug0)
simplicialNodes(ug0)
#skladowe spojnosci
connComp(ug0)
ug0 <- removeEdge("1","4", ug0)
connComp(ug0)

#gRbase domyslnie tworzy grafy w formacie graphNEL
#grafy latwo mozna mapowac na inne typy
ug1 <- ug(c(1,2,3),c(1,3,4),c(5),result="igraph") #igraph
try(dev.off(dev.list()["RStudioGD"]), silent=TRUE)
plot(ug1)

ug2 <- ug(c(1,2),c(2,3,4),c(5),result="matrix") #adjacency matrix
ug2
#i obrabiac w innych pakietach
qgraph::qgraph(ug2)

#podobnie z grafami skierowanymi (dag=directed acyclic graph)
dag0 <- dag(c(1),c(2,1), c(3,1,2), c(4,3,5), c(5,1), c(7,6)) #listujemy kolejno (i, parents(i))
try(dev.off(dev.list()["RStudioGD"]), silent=TRUE)
plot(dag0)

dag1 <- dag(c(1),c(2,1), c(3,1,2), c(4,3,5), c(5,1), c(7,6), result="matrix") #macierz incydencji
dag1
#mamy prosty dostep do rodzicow i dzieci kazdego wierzcholka
parents("4",dag0)
children("3",dag0)
#oraz np do zbiorow przodkow:
ancestralSet(c("4"),dag0)
ancestralSet(c("2","5"),dag0)



#przyklad analizy grafowej
if (!requireNamespace("sparseIndexTracking", quietly = TRUE))
  install.packages("sparseIndexTracking")
if (!requireNamespace("xts", quietly = TRUE))
  install.packages("xts")

library(sparseIndexTracking); 
library(xts)
data(INDEX_2010)
data <- INDEX_2010$X[,101:200] #n=252, p=100
G <- qgraph::qgraph(cor(data),graph="glasso",sampleSize=nrow(data),layout="spring",threshold=.07) #trwa chwile, nie przejmowac sie warningiem

Gi <- as.igraph(G)

edge_density(Gi, loops=F) #iloraz liczby wierzcholkow do wszystkich mozliwych polaczen
diameter(Gi, directed=F, weights=NA) #stosunkowo mala srednica
deg <- igraph::degree(Gi)
which(deg==max(deg)) #mozemy wylistowac wierzcholki o najwiekszym stopniu, ale sprobujmy je zobaczyc

#narysujemy graf ktorego rozmiar wierzcholka i kolor zaleza od stopnia
layout <- G$layout 
try(dev.off(dev.list()["RStudioGD"]), silent=TRUE)
plot(Gi, vertex.size=deg*2,vertex.color=deg,vertex.label=NA,layout=layout)

#many networks consist of modules which are densely connected themselves but sparsely connected to other modules.
E(Gi)$weight <- abs(E(Gi)$weight)
ceb <- cluster_edge_betweenness(Gi,directed=FALSE) #olewamy warning
plot(ceb, Gi,vertex.label.cex=.2,layout=layout)

