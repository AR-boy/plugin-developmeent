import sys
import cv2
from cv2 import aruco

if __name__ == '__main__':
    if len(sys.argv) - 1 != 4 :
        print("incorrect number of arguments - arguments necessary are: number of squaresW, squaresH, markerLength and markerSeperation")
    else :
        
        numOfSquaresW = sys.argv[1]
        numOfSquaresH = sys.argv[2]
        markerLength = sys.argv[3]
        markerSeperation = sys.argv[4]

        aruco_dict =  aruco.Dictionary_get(aruco.DICT_4X4_50)
        aruco_board = aruco.GridBoard_create(int(numOfSquaresW), int(numOfSquaresH), float(markerLength), float(markerSeperation), aruco_dict)
        aruco_board_image = aruco_board.draw((2000, 2000))
        cv2.imwrite(("ArucoBoard_"+numOfSquaresW+"_"+numOfSquaresH+"_"+markerLength+"_"+markerSeperation+".png"), aruco_board_image)
        print("generated ArucoBoard image")