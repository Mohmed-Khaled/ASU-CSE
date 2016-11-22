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
            w = (ord(msg[i]) << 8) + ord(msg[i + 1])
        s = carry_around_add(s, w)
    return s & 0xffff


server_port = 12000
server_socket = socket(AF_INET, SOCK_DGRAM)
server_socket.settimeout(1)
server_socket.bind(('', server_port))
print "The server is ready to receive"
expected_pkt_no = 0
last_ack = "None"
while 1:
    try:
        packet, clientAddress = server_socket.recvfrom(2048)
        received_checksum = packet[0:4]
        pkt_no = int(packet[4:5], 16)
        message = packet[5:].decode("hex")
        calculated_checksum = checksum(message)
        not_corrupted = (int(received_checksum, 16) + calculated_checksum) == 65535
        correct_sequence = pkt_no == expected_pkt_no
        if not_corrupted and correct_sequence:
            if expected_pkt_no == 0:
                last_ack = "ACK0"
                expected_pkt_no = (expected_pkt_no + 1) % 2
            else:
                last_ack = "ACK1"
                expected_pkt_no = (expected_pkt_no + 1) % 2
            print "A message with sequence: ", pkt_no, " from (", clientAddress, "): ", message
        sleep(random.uniform(0, 2))
        server_socket.sendto(last_ack, clientAddress)
    except error:
        continue

