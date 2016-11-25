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
server_socket.settimeout(0.5)
server_socket.bind(('', server_port))
print "The server is ready to receive"
expected_seq_no = 1
last_ack = 0
while 1:
    try:
        packet, clientAddress = server_socket.recvfrom(2048)
        received_checksum = packet[0:4]
        seq_no = int(packet[4:12], 16)
        message = packet[12:].decode("hex")
        calculated_checksum = checksum(message)
        not_corrupted = (int(received_checksum, 16) + calculated_checksum) == 65535
        correct_sequence = seq_no == expected_seq_no
        if not_corrupted and correct_sequence:
            expected_seq_no += len(message)
            print "A message with sequence: ", seq_no, " from (", clientAddress, "): ", message
        sleep(random.uniform(0, 2))
        server_socket.sendto(str(expected_seq_no), clientAddress)
    except error:
        continue

