from socket import *
from time import sleep
import random


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


next_seq_num = 1
send_base = 1


def tcp_send(message, destination_ip, destination_port):
    global next_seq_num
    global send_base
    message_bytes = [elem.encode("hex") for elem in message]
    data = ''.join(message_bytes)
    sndpkt = format(checksum(message), 'x') + format(next_seq_num, '08x') + data  # make_pkt
    client_socket = socket(AF_INET, SOCK_DGRAM)
    client_socket.settimeout(0.5)
    while 1:
        print "sending pkt with seq:", next_seq_num
        client_socket.sendto(sndpkt,(destination_ip,destination_port))  # udt_send
        try:
            response, server_address = client_socket.recvfrom(2048)
            print "ACK seq:", response
            if int(response) == next_seq_num + len(message):
                next_seq_num = int(response)
                client_socket.close()
                return
        except timeout:
            print "timeout"
            continue
        except error, e:
            continue

while 1:
    str_message = "test message"
    tcp_send(str_message, 'localhost', 12000)
    sleep(random.uniform(0, 2))