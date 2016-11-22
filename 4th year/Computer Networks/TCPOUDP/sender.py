from socket import *
from time import sleep


def carry_around_add(a, b):
    c = a + b
    return (c & 0xffff) + (c >> 16)


def checksum(msg):
    s = 0
    for i in range(0, len(msg), 2):
        if i == len(msg) - 1:
            w = (ord(msg[i]) << 8) + 0x0000
        else:
            w = (ord(msg[i]) << 8) + ord(msg[i+1])
        s = carry_around_add(s, w)
    return ~s & 0xffff


pkt_no = 0


def rdt_send(message, destination_ip, destination_port):
    """rdt 3.0"""
    global pkt_no
    message_bytes = [elem.encode("hex") for elem in message]
    data = ''.join(message_bytes)
    sndpkt = format(checksum(message), 'x') + format(pkt_no, 'x') + data  # make_pkt
    client_socket = socket(AF_INET, SOCK_DGRAM)
    client_socket.settimeout(1)
    while 1:
        print "sending pkt", pkt_no
        client_socket.sendto(sndpkt,(destination_ip,destination_port))  # udt_send
        try:
            response, server_address = client_socket.recvfrom(2048)
            if pkt_no == 0:
                if response == "ACK0":
                    client_socket.close()
                    pkt_no = (pkt_no + 1) % 2
                    return
            else:
                if response == "ACK1":
                    client_socket.close()
                    pkt_no = (pkt_no + 1) % 2
                    return
        except timeout:
            print "timeout"
            continue
        except error, e:
            continue

while 1:
    str_message = "test message"
    rdt_send(str_message, 'localhost', 12000)
    sleep(2)