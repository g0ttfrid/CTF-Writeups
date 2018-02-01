# broken msg: “Mostre seu conhecimento em análise de pacotes e encontre a flag”

> https://shellterlabs.com/en/questions/arahacker-day-2017/broken-msg/

O desafio nos fornece um arquivo com a extensão pcapng, vamos conferir:
```shell
$ file net.pcapng
net.pcapng: pcap-ng capture file – version 1.0
```

Abri o arquivo com o wireshark, ao analisar os primeiros pacotes TCP com 'tcp follow' encontramos uma trollagem. Extraio o conteúdo do pcap com o tcpflow para verificar todos os dados que o tcpflow consiga extrair:
```shell
tcpflow -r net.pcapng
```

Ao exibir o conteúdo dos arquivos encontramos a trollagem e o que parece ser três pedaços de um hash, juntando e decodando em base32 obtenho a flag.

```shell
$ cat 192.168.001.19*
tranquilidade
procura aew man kkk
aew blz
e a flag...
NYZXI5ZQ
OJVV6MZU
ON4Q====

$ echo "NYZXI5ZQOJVV6MZUON4Q====" | base32 -d
*******_****
```


Obrigado!
